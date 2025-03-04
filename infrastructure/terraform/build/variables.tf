# Regions
variable "Region" {
  description = "AWS depoloyment region"
  type        = string
}

# Network variable
variable "vpc_id" {
  description = "id of vpc used to deploy"
  type = string
}
variable "SSLCertificateARN"{
  description = "SSL certificate for https traffic"
  type        = string
}
# Tagging and naming
variable "Application" {
  description = "Application used to name all resources"
  type        = string
}
variable "SolTag" {
  description = "Solution tag value. All resources are created with a 'Solution' tag name and the value you set here"
  type        = string
}
variable "EnvCode" {
  description = "2 character code used to name all resources e.g. 'pd' for production"
  type        = string
}
variable "EnvTag" {
  description = "Environment tag value. All resources are created with an 'Environment' tag name and the value you set here"
  type        = string
}

# Web App Build
variable "ArtifactoryRepo" {
  description = "Name of Artifactory repository"
  type        = string
}
variable "ImageTag" {
  description = "Docker Image Tag"
  type        = string
}