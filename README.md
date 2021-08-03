# cinema
Teste Prático PrintWayy

1: Alterar as configurações de banco de dados no arquivo "appsettings.json" que em meu projeto foi realizado através do SQLEXPRESS,
onde "Data Source" é o nome do servidor do banco e "Initial Catalog" é o banco de dados.
	...
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=?;Initial Catalog=?;Integrated Security=True"
		...
	...

2: Executar as migrations na pasta "/Migrations"
- 20210724191019_InitialMigration
- 20210724191035_InsertMigration
- 20210726004426_AddIdentity

3: Ao iniciar o programa, automaticamente será executado o recurso "Seed Data", onde será populado as Roles "Manager" e "Collaborator" bem como seus usuários/senhas padrões conforme
informações contidas no arquivo "appsettings.json", onde "Manager" é o Super usuário administrador e "Collaborator" é o usuário padrão do sistema.
	...
	"UserSettings": {
        "Manager": {
            "UserName": "manager",
            "UserEmail": "manager@gmail.com",
            "UserPassword": "!@#Senha123"
        },
        "Collaborator": {
            "UserName": "collaborator",
            "UserEmail": "collaborator@gmail.com",
            "UserPassword": "!@#Senha123"
        }
    },
	...
