# JobSityChat

## Developer:
Name: Manuel Guerrero

LinkedIn: https://www.linkedin.com/in/manuel-guerrero-070144166/

## Technologies used

- .Net 5.0
- Asp.Net Core 5.0
- SignalR
- Bootstrap 
- SQLite
- RabbitMQ
- Entity Framework Core
- TinyCsvParser
- Docker
- Blazor Server (With .Net 5)

## Instrucctions to install the application :

- Download the project or clone it
- Install SQLite, you can have it from here: (https://www.sqlite.org/download.html)
- Install Net 5, you can have it from here: (https://dotnet.microsoft.com/download/dotnet/5.0)
- Install Docker, you can have it from here: (https://www.docker.com/products/docker-desktop)
- Use Docker to install RammitMQ, the command will be like this:
  ````
	docker run -d --hostname jobsity-chat --name jobsity -p 15672:15672 -p 5672:5672 rabbitmq:3-management 
	````

## How to run the application :

- Run RammitMQ, you can do it executing the command above:
  ````
	docker run -d --hostname jobsity-chat --name jobsity -p 15672:15672 -p 5672:5672 rabbitmq:3-management 
	````
- Open the JobSityChat.BE project
  - Set the JobSityChat.Api and the JobSityChat.StockBot as strtup projects
  - Run the solution

- Open the JobSityChat.UI project and run the application

Now, create a new account going to the register page, confirm your email and then log in.
After that you can start using the chat.

**Note:** If you see a message that said that there is a problem with the Queue, that means that the client and the backend could not be connected through SingalR, you need to run the Api first and then the client.


  
