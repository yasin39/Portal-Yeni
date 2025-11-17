using Portal.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Portal
{
    public partial class DatabaseHealth : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Yetki kontrolü: Sadece Admin görebilir (yetkiNo: 100)
            if (!CheckPermission(100))
            {
                return;
            }

            if (!IsPostBack)
            {
                LoadDatabaseHealthData();
            }
        }

        #region Model Sınıfları

        /// <summary>
        /// Database genel istatistikleri
        /// </summary>
        public class DatabaseStatistics
        {
            public int TotalTables { get; set; }
            public decimal DatabaseSizeGB { get; set; }
            public int ActiveConnections { get; set; }
            public int SlowQueriesCount { get; set; }
        }

        /// <summary>
        /// Tablo boyut bilgisi
        /// </summary>
        public class TableSizeInfo
        {
            public string TableName { get; set; }
            public long RowCount { get; set; }
            public decimal DataSizeMB { get; set; }
            public decimal IndexSizeMB { get; set; }
            public decimal TotalSizeMB { get; set; }
        }

        /// <summary>
        /// Kullanılmayan index bilgisi
        /// </summary>
        public class UnusedIndexInfo
        {
            public string TableName { get; set; }
            public string IndexName { get; set; }
        }

        /// <summary>
        /// Eksik index önerisi
        /// </summary>
        public class MissingIndexInfo
        {
            public string TableName { get; set; }
            public string EqualityColumns { get; set; }
            public string IncludeColumns { get; set; }
            public decimal ImprovementPercent { get; set; }
        }

        /// <summary>
        /// Connection pool bilgisi
        /// </summary>
        public class ConnectionPoolInfo
        {
            public int TotalConnections { get; set; }
            public int ActiveConnections { get; set; }
            public int IdleConnections { get; set; }
        }

        /// <summary>
        /// Yavaş query bilgisi
        /// </summary>
        public class SlowQueryInfo
        {
            public string QueryText { get; set; }
            public int ExecutionTimeMs { get; set; }
            public int ExecutionCount { get; set; }
        }

        /// <summary>
        /// Backup geçmişi bilgisi
        /// </summary>
        public class BackupHistoryInfo
        {
            public DateTime BackupDate { get; set; }
            public string BackupType { get; set; }
            public decimal BackupSizeMB { get; set; }
        }

        #endregion

        #region Veri Yükleme

        /// <summary>
        /// Tüm database sağlık verilerini yükler
        /// </summary>
        private void LoadDatabaseHealthData()
        {
            try
            {
                // 1. Genel istatistikler
                var stats = GetDatabaseStatistics();
                lblTotalTables.Text = stats.TotalTables.ToString();
                lblDatabaseSize.Text = stats.DatabaseSizeGB.ToString("0.00");
                lblActiveConnections.Text = stats.ActiveConnections.ToString();
                lblSlowQueries.Text = stats.SlowQueriesCount.ToString();

                // 2. Connection pool bilgileri
                var connPool = GetConnectionPoolInfo();
                lblTotalConnections.Text = connPool.TotalConnections.ToString();
                lblActiveConn.Text = connPool.ActiveConnections.ToString();
                lblIdleConn.Text = connPool.IdleConnections.ToString();

                // 3. Son backup bilgisi
                var lastBackup = GetLastBackupInfo();
                if (lastBackup != null)
                {
                    lblLastBackupDate.Text = lastBackup.BackupDate.ToString("dd.MM.yyyy HH:mm");
                    lblLastBackupType.Text = lastBackup.BackupType;
                    lblLastBackupSize.Text = lastBackup.BackupSizeMB.ToString("0.00") + " MB";
                }

                // 4. En büyük tablolar (Chart için JSON)
                var largestTables = GetLargestTables(15);
                hdnTableSizeData.Value = SerializeTableSizeData(largestTables);

                // 5. En büyük 20 tablo (Repeater için)
                rptLargestTables.DataSource = GetLargestTables(20);
                rptLargestTables.DataBind();

                // 6. Kullanılmayan indexler
                rptUnusedIndexes.DataSource = GetUnusedIndexes();
                rptUnusedIndexes.DataBind();

                // 7. Eksik index önerileri
                rptMissingIndexes.DataSource = GetMissingIndexes();
                rptMissingIndexes.DataBind();

                // 8. Yavaş query'ler
                rptSlowQueries.DataSource = GetSlowQueries();
                rptSlowQueries.DataBind();

                // 9. Backup geçmişi
                rptBackupHistory.DataSource = GetBackupHistory(10);
                rptBackupHistory.DataBind();
            }
            catch (Exception ex)
            {
                LogError("LoadDatabaseHealthData hatası", ex);
                ShowToast("Database sağlık verileri yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Database genel istatistiklerini getirir
        /// </summary>
        private DatabaseStatistics GetDatabaseStatistics()
        {
            var stats = new DatabaseStatistics();

            try
            {
                // Toplam tablo sayısı
                string tableCountQuery = "SELECT COUNT(*) FROM sys.tables WHERE is_ms_shipped = 0";
                stats.TotalTables = Convert.ToInt32(ExecuteScalar(tableCountQuery));

                // Database boyutu
                string sizeQuery = @"
                    SELECT 
                        SUM(size) * 8.0 / 1024 / 1024 AS SizeGB
                    FROM sys.master_files
                    WHERE database_id = DB_ID()";

                object sizeResult = ExecuteScalar(sizeQuery);
                stats.DatabaseSizeGB = sizeResult != null && sizeResult != DBNull.Value
                    ? Convert.ToDecimal(sizeResult)
                    : 0;

                // Aktif bağlantı sayısı
                string connQuery = @"
                    SELECT COUNT(*) 
                    FROM sys.dm_exec_connections 
                    WHERE database_id = DB_ID()";
                stats.ActiveConnections = Convert.ToInt32(ExecuteScalar(connQuery));

                // Yavaş query sayısı (son 24 saat)
                string slowQueryQuery = @"
                    SELECT COUNT(DISTINCT query_hash)
                    FROM sys.dm_exec_query_stats
                    WHERE total_elapsed_time / execution_count > 1000000
                    AND last_execution_time > DATEADD(HOUR, -24, GETDATE())";

                object slowQueryResult = ExecuteScalar(slowQueryQuery);
                stats.SlowQueriesCount = slowQueryResult != null && slowQueryResult != DBNull.Value
                    ? Convert.ToInt32(slowQueryResult)
                    : 0;
            }
            catch (Exception ex)
            {
                LogError("GetDatabaseStatistics hatası", ex);
            }

            return stats;
        }

        /// <summary>
        /// En büyük N tablo getirir
        /// </summary>
        private List<TableSizeInfo> GetLargestTables(int topCount)
        {
            var tables = new List<TableSizeInfo>();

            try
            {
                string query = $@"
                    SELECT TOP {topCount}
                        t.NAME AS TableName,
                        p.rows AS RowCount,
                        CAST(ROUND((SUM(a.used_pages) / 128.00), 2) AS DECIMAL(18,2)) AS TotalSizeMB,
                        CAST(ROUND((SUM(CASE WHEN a.type <> 1 THEN a.used_pages ELSE 0 END) / 128.00), 2) AS DECIMAL(18,2)) AS IndexSizeMB,
                        CAST(ROUND((SUM(CASE WHEN a.type = 1 THEN a.used_pages ELSE 0 END) / 128.00), 2) AS DECIMAL(18,2)) AS DataSizeMB
                    FROM 
                        sys.tables t
                    INNER JOIN      
                        sys.indexes i ON t.OBJECT_ID = i.object_id
                    INNER JOIN 
                        sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
                    INNER JOIN 
                        sys.allocation_units a ON p.partition_id = a.container_id
                    WHERE 
                        t.is_ms_shipped = 0
                        AND i.OBJECT_ID > 255
                    GROUP BY 
                        t.NAME, p.Rows
                    ORDER BY 
                        TotalSizeMB DESC";

                DataTable dt = ExecuteDataTable(query);

                foreach (DataRow row in dt.Rows)
                {
                    tables.Add(new TableSizeInfo
                    {
                        TableName = row["TableName"].ToString(),
                        RowCount = Convert.ToInt64(row["RowCount"]),
                        TotalSizeMB = Convert.ToDecimal(row["TotalSizeMB"]),
                        IndexSizeMB = Convert.ToDecimal(row["IndexSizeMB"]),
                        DataSizeMB = Convert.ToDecimal(row["DataSizeMB"])
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetLargestTables hatası", ex);
            }

            return tables;
        }

        /// <summary>
        /// Kullanılmayan indexleri getirir
        /// </summary>
        private List<UnusedIndexInfo> GetUnusedIndexes()
        {
            var indexes = new List<UnusedIndexInfo>();

            try
            {
                string query = @"
                    SELECT TOP 10
                        OBJECT_NAME(i.object_id) AS TableName,
                        i.name AS IndexName
                    FROM sys.indexes i
                    LEFT JOIN sys.dm_db_index_usage_stats s 
                        ON i.object_id = s.object_id 
                        AND i.index_id = s.index_id 
                        AND s.database_id = DB_ID()
                    WHERE OBJECTPROPERTY(i.object_id, 'IsUserTable') = 1
                        AND i.index_id > 1
                        AND s.index_id IS NULL
                    ORDER BY OBJECT_NAME(i.object_id)";

                DataTable dt = ExecuteDataTable(query);

                foreach (DataRow row in dt.Rows)
                {
                    indexes.Add(new UnusedIndexInfo
                    {
                        TableName = row["TableName"].ToString(),
                        IndexName = row["IndexName"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetUnusedIndexes hatası", ex);
            }

            return indexes;
        }

        /// <summary>
        /// Eksik index önerilerini getirir
        /// </summary>
        private List<MissingIndexInfo> GetMissingIndexes()
        {
            var indexes = new List<MissingIndexInfo>();

            try
            {
                string query = @"
                    SELECT TOP 10
                        OBJECT_NAME(d.object_id) AS TableName,
                        d.equality_columns AS EqualityColumns,
                        d.included_columns AS IncludeColumns,
                        CAST(s.avg_user_impact AS DECIMAL(5,2)) AS ImprovementPercent
                    FROM sys.dm_db_missing_index_details d
                    INNER JOIN sys.dm_db_missing_index_groups g 
                        ON d.index_handle = g.index_handle
                    INNER JOIN sys.dm_db_missing_index_group_stats s 
                        ON g.index_group_handle = s.group_handle
                    WHERE d.database_id = DB_ID()
                    ORDER BY s.avg_user_impact DESC";

                DataTable dt = ExecuteDataTable(query);

                foreach (DataRow row in dt.Rows)
                {
                    indexes.Add(new MissingIndexInfo
                    {
                        TableName = row["TableName"].ToString(),
                        EqualityColumns = row["EqualityColumns"]?.ToString() ?? "-",
                        IncludeColumns = row["IncludeColumns"]?.ToString() ?? "-",
                        ImprovementPercent = row["ImprovementPercent"] != DBNull.Value
                            ? Convert.ToDecimal(row["ImprovementPercent"])
                            : 0
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetMissingIndexes hatası", ex);
            }

            return indexes;
        }

        /// <summary>
        /// Connection pool bilgilerini getirir
        /// </summary>
        private ConnectionPoolInfo GetConnectionPoolInfo()
        {
            var info = new ConnectionPoolInfo();

            try
            {
                string query = @"
                    SELECT 
                        COUNT(*) AS TotalConnections,
                        SUM(CASE WHEN status = 'running' THEN 1 ELSE 0 END) AS ActiveConnections,
                        SUM(CASE WHEN status = 'sleeping' THEN 1 ELSE 0 END) AS IdleConnections
                    FROM sys.dm_exec_sessions
                    WHERE database_id = DB_ID()
                        AND is_user_process = 1";

                DataTable dt = ExecuteDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    info.TotalConnections = dt.Rows[0]["TotalConnections"] != DBNull.Value
                        ? Convert.ToInt32(dt.Rows[0]["TotalConnections"])
                        : 0;

                    info.ActiveConnections = dt.Rows[0]["ActiveConnections"] != DBNull.Value
                        ? Convert.ToInt32(dt.Rows[0]["ActiveConnections"])
                        : 0;

                    info.IdleConnections = dt.Rows[0]["IdleConnections"] != DBNull.Value
                        ? Convert.ToInt32(dt.Rows[0]["IdleConnections"])
                        : 0;
                }
            }
            catch (Exception ex)
            {
                LogError("GetConnectionPoolInfo hatası", ex);
            }

            return info;
        }

        /// <summary>
        /// Yavaş query'leri getirir (>1000ms, son 24 saat)
        /// </summary>
        private List<SlowQueryInfo> GetSlowQueries()
        {
            var queries = new List<SlowQueryInfo>();

            try
            {
                string query = @"
                    SELECT TOP 10
                        SUBSTRING(qt.text, (qs.statement_start_offset/2)+1,
                            ((CASE qs.statement_end_offset
                                WHEN -1 THEN DATALENGTH(qt.text)
                                ELSE qs.statement_end_offset
                            END - qs.statement_start_offset)/2) + 1) AS QueryText,
                        CAST((qs.total_elapsed_time / qs.execution_count) / 1000 AS INT) AS ExecutionTimeMs,
                        qs.execution_count AS ExecutionCount
                    FROM sys.dm_exec_query_stats qs
                    CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
                    WHERE qs.total_elapsed_time / qs.execution_count > 1000000
                        AND qs.last_execution_time > DATEADD(HOUR, -24, GETDATE())
                    ORDER BY ExecutionTimeMs DESC";

                DataTable dt = ExecuteDataTable(query);

                foreach (DataRow row in dt.Rows)
                {
                    string queryText = row["QueryText"]?.ToString() ?? "";

                    // Query text'i temizle ve kısalt
                    queryText = queryText.Trim();
                    if (queryText.Length > 200)
                    {
                        queryText = queryText.Substring(0, 200) + "...";
                    }

                    queries.Add(new SlowQueryInfo
                    {
                        QueryText = queryText,
                        ExecutionTimeMs = Convert.ToInt32(row["ExecutionTimeMs"]),
                        ExecutionCount = Convert.ToInt32(row["ExecutionCount"])
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetSlowQueries hatası", ex);
            }

            return queries;
        }

        /// <summary>
        /// Son backup bilgisini getirir
        /// </summary>
        private BackupHistoryInfo GetLastBackupInfo()
        {
            BackupHistoryInfo info = null;

            try
            {
                string query = @"
                    SELECT TOP 1
                        backup_finish_date AS BackupDate,
                        CASE type
                            WHEN 'D' THEN 'Full'
                            WHEN 'I' THEN 'Differential'
                            WHEN 'L' THEN 'Log'
                            ELSE 'Other'
                        END AS BackupType,
                        CAST(backup_size / 1024.0 / 1024.0 AS DECIMAL(10,2)) AS BackupSizeMB
                    FROM msdb.dbo.backupset
                    WHERE database_name = DB_NAME()
                    ORDER BY backup_finish_date DESC";

                DataTable dt = ExecuteDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    info = new BackupHistoryInfo
                    {
                        BackupDate = Convert.ToDateTime(dt.Rows[0]["BackupDate"]),
                        BackupType = dt.Rows[0]["BackupType"].ToString(),
                        BackupSizeMB = Convert.ToDecimal(dt.Rows[0]["BackupSizeMB"])
                    };
                }
            }
            catch (Exception ex)
            {
                LogError("GetLastBackupInfo hatası", ex);
            }

            return info;
        }

        /// <summary>
        /// Backup geçmişini getirir
        /// </summary>
        private List<BackupHistoryInfo> GetBackupHistory(int topCount)
        {
            var backups = new List<BackupHistoryInfo>();

            try
            {
                string query = $@"
                    SELECT TOP {topCount}
                        backup_finish_date AS BackupDate,
                        CASE type
                            WHEN 'D' THEN 'Full'
                            WHEN 'I' THEN 'Differential'
                            WHEN 'L' THEN 'Log'
                            ELSE 'Other'
                        END AS BackupType,
                        CAST(backup_size / 1024.0 / 1024.0 AS DECIMAL(10,2)) AS BackupSizeMB
                    FROM msdb.dbo.backupset
                    WHERE database_name = DB_NAME()
                    ORDER BY backup_finish_date DESC";

                DataTable dt = ExecuteDataTable(query);

                foreach (DataRow row in dt.Rows)
                {
                    backups.Add(new BackupHistoryInfo
                    {
                        BackupDate = Convert.ToDateTime(row["BackupDate"]),
                        BackupType = row["BackupType"].ToString(),
                        BackupSizeMB = Convert.ToDecimal(row["BackupSizeMB"])
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetBackupHistory hatası", ex);
            }

            return backups;
        }

        #endregion

        #region Yardımcı Metodlar

        /// <summary>
        /// Tablo boyut verisini JSON'a çevirir (Chart.js için)
        /// </summary>
        private string SerializeTableSizeData(List<TableSizeInfo> tables)
        {
            try
            {
                var labels = tables.Select(t => t.TableName).ToList();
                var data = tables.Select(t => t.TotalSizeMB).ToList();

                var json = new
                {
                    labels = labels,
                    datasets = new[]
                    {
                        new {
                            label = "Boyut (MB)",
                            data = data,
                            backgroundColor = new[]
                            {
                                "#667eea", "#764ba2", "#43e97b", "#38f9d7", "#ffa726",
                                "#fb8c00", "#f85032", "#e73827", "#4facfe", "#00f2fe",
                                "#667eea", "#764ba2", "#43e97b", "#38f9d7", "#ffa726"
                            }
                        }
                    }
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(json);
            }
            catch
            {
                return "{}";
            }
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Yenile butonu
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDatabaseHealthData();
            ShowToast("Database sağlık verileri yenilendi.", "success");
        }

        #endregion
    }
}
