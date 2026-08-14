output "kubernetes_cluster_name" {
  value = azurerm_kubernetes_cluster.aks.name
}

output "kubernetes_cluster_host" {
  value     = azurerm_kubernetes_cluster.aks.kube_config.0.host
  sensitive = true
}

output "sql_server_fqdn" {
  value = azurerm_mssql_server.sql_server.fully_qualified_domain_name
}
