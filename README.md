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
- Install Docker, you can have it from here: (https://www.docker.com/products/docker-desktop)
- Use Docker to run the application, the command will be like this:
  ````
	docker-compose up --build
	````

## Instrucctions to use the application :

- Register a new account and confirm the email.
- Go to the home page and start messaging.


**Note:** If you see a message that said that there is a problem with the Queue, that means that the client and the backend could not be connected through SingalR, you need to run the Api first and then the client.

Here I have private and short video of how run the application: https://youtu.be/rrgLKBNStOE
