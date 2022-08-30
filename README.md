# DevEnvironment-RabbitMq  
Exemplo de ambiente de DEV usando RabbitMq com docker.

## Como usar  

Para subir um servidor local, basta executar via PowerShell:  

```console
.\create-rabbit.ps1
```

Por padrão, o site de administração do RabbitMq irá subir na porta 1567.  
http://localhost:15672  

**User:** admin
**Password:** admin

Para destruir o container:  

```console
.\destroy-rabbit.ps1
```