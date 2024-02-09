# CLI Commands Microservices

Consisting of 2 microservices - Platform and Commands Service. Platform service contains platform names and Commands service contains the CLI commands for the platforms from platforms service. The system follows Event - driven architecture, both services communicate via an Event bus - RabbitMQ in this case and uses gRPC as the RPC framework for syncing platforms between services. Both services are dockerized and the images are stored in Docker hub. These containers are then orchestrated using Kubernetes and yaml deployment files.

# Tech Stack & Architecture
* ASP.NET Core
* Docker
* Kubernetes
* RabbitMQ
* gRPC
* Entity Framework Core
* SQL Server

# Credits
Les Jackson's github repo and course URL - https://github.com/binarythistle/S04E03---.NET-Microservices-Course-, https://youtu.be/DgVjEo3OGBI
