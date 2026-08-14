variable "resource_group_name" {
  type        = string
  default     = "rg-recruitment-dev"
  description = "Azure Resource Group Name"
}

variable "location" {
  type        = string
  default     = "eastus"
  description = "Azure Region Location"
}

variable "cluster_name" {
  type        = string
  default     = "recruitment-aks-cluster"
  description = "AKS Cluster Name"
}

variable "node_count" {
  type        = number
  default     = 2
  description = "Number of AKS Worker Nodes"
}

variable "environment" {
  type        = string
  default     = "dev"
  description = "Environment stage (dev, staging, prod)"
}

variable "db_admin_user" {
  type        = string
  default     = "recruitmentadmin"
  description = "SQL Server Admin User"
}

variable "db_admin_password" {
  type        = string
  sensitive   = true
  default     = "SecureP@ssw0rd2026!"
  description = "SQL Server Admin Password"
}
