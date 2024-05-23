# API_UpdateServiceDocker
Uma API que a partir de uma requisição, atualizar um container docker no linux

Comando para fazer build da imagem docker:  

sudo docker build -t updateservicedocker . 

Comando para criar container: 

sudo docker run -d --restart always -v /var/run/docker.sock:/var/run/docker.sock  -v /usr/bin/docker:/host/docker -v /home/eduardo/Files-Data:/Files-Data/ -e PATH=$PATH:/host -p 8080:8080 --name UpdateServiceDocker updateservicedocker 

Algumas informações sobre o comando acima: 

--restart always => ele sempre reiniciar caso o servidor reiniciar ou se der algum erro. 
-v /var/run/docker.sock:/var/run/docker.sock => para consegui executar o docker (Error: Cannot connect to the Docker daemon at unix:///var/run/docker.sock. Is the docker daemon running?) 
-v /usr/bin/docker:/host/docker => para conseguir acessar os arquivos do docker 
-e PATH=$PATH:/host  => Para não precisar ficar digitando /host/docker no terminal quando for usar algum comando docker 
-v /home/eduardo/Files-Data:/Files-Data/ => Configurações da api 

Tem um exemplo do diretório Files-Data que é utilizado, dentro dele temos um json chamado config.json, nele definimos o nome do arquivo .sh que iremos utilizar e um nome para “apelidar” o script na hora que formos chamar. 
 
Junto do arquivo config.json, também temos uma pasta denominada “Scripts”, nela será onde iremos armazenar os scripts para serem executados, no caso como mostra no print acima, temos 2 arquivos “scriptUpdate_8081.sh” e “scriptUpdate_8084.sh”, ambos têm quase os mesmos comandos dentro do script, uma série de comandos para atualizar um container no docker, única diferença e o nome dos containers  

Para executar o script pela api será precisa fazer uma requisição a via http com o método PUT para a url http://localhost:8080/update/8081 , lembrando de alterar o localhost para o ip do servidor e o 8080 para a porta correta da api, além de mudar o 8081 para o nome definido dentro do config.json para o script desejado. Nesse caso eu usei o 8081 para ser como se fosse a porta utilizada pela api que será atualizado, mas você pode colocar outro nome da sua preferência e organização. 

Para realizar essa requisição no Jenkins, deixei ele configurado para “Executar no comando do Windows” e usei o seguinte script: 

@echo off 

setlocal enabledelayedexpansion 

:: Faz a requisição HTTP e armazena o código de status na variável 

for /f "tokens=* delims=" %%i in ('curl --write-out "%%{http_code}" --silent --output NUL --request PUT --url http://192.168.0.111:8080/update/8081') do set "response=%%i" 

:: Verifica se o código de status é diferente de 200 

if "%response%" NEQ "200" ( 

    echo Erro na requisição: Código HTTP %response% 
    
    exit /b 1 
    
) else ( 

    echo Requisição bem-sucedida: Código HTTP %response% 
    
) 


Com esse script, se a requisição der um código diferente de “200”, ele informar no histórico do Jenkins que houve um erro: 
