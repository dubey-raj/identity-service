# Create a security group for RDS
resource "aws_security_group" "rds_security_group" {
  name        = "rds_security_group"
  description = "Allow inbound traffic to PostgreSQL"

  ingress {
    from_port   = 5432
    to_port     = 5432
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] # Change to your trusted IPs or VPC CIDR
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Create an RDS PostgreSQL instance
resource "aws_db_instance" "postgres" {
  allocated_storage    = 20                         # Storage in GB
  storage_type         = "gp2"                      # General Purpose SSD
  engine               = "postgres"                # Specify PostgreSQL
  engine_version       = "17.2"                    # Specify PostgreSQL version
  instance_class       = "db.t3.micro"              # Instance size
  db_name              = "users_db"              # Database name
  username             = "pgadmin"                   # Master username
  password             = "supersecurepassword123"  # Master password
  parameter_group_name = "default.postgres14"      # Parameter group
  publicly_accessible  = true                      # Set to false for private DBs
  vpc_security_group_ids = [aws_security_group.rds_security_group.id] # Security group
  skip_final_snapshot  = true                      # Prevents snapshot on delete
  backup_retention_period = 7                      # Retain backups for 7 days
  multi_az             = false                     # Disable Multi-AZ for cost saving
}

# Output PostgreSQL endpoint
output "postgres_endpoint" {
  value = aws_db_instance.postgres.endpoint
}
