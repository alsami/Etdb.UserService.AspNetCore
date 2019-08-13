#!/usr/bin/env bash
cleanup_dir() {
    echo removing old directory
    sudo rm -rf /var/www/Etdb.UserService.AspNetCore
}

publish_service() {
    echo publishing service
    sudo dotnet publish --runtime linux-x64 --self-contained --output /var/www/Etdb.UserService.AspNetCore
    echo copying service definition file
    sudo cp etdb.userservice.aspnetcore.service /etc/systemd/system/etdb.userservice.aspnetcore.service
}

enable_service() {
    echo enabling service
    sudo systemctl enable etdb.userservice.aspnetcore.service
    echo starting service
    sudo systemctl start etdb.userservice.aspnetcore.service
    sudo systemctl restart etdb.userservice.aspnetcore.service
    sleep 5
    echo printing aspnetcore service status
    sudo systemctl status etdb.userservice.aspnetcore.service
}

download_swagger_file() {
    echo downloading swagger definition
    curl -v http://localhost:5000/swagger/v1/swagger.json > swagger.json
}

setup_git_user() {
    git config --global user.name "Travis CI" && git config --global user.email "travis@travis-ci.com"
}

clone_client_repo() {
    echo cloning repository
    git clone https://github.com/entertainment-database/Etdb.UserService.Client.CSharp.git
}

install_node() {
    echo installing nodejs
    sudo apt-get install libuv1 -y
    sudo apt-get install nodejs -y 
}

install_auto_rest() {
    echo installing autorest
    sudo npm i -g autorest@beta
    sudo autorest
}

create_csharp_client() {
    echo creating csharp-client
    sudo autorest --csharp --input-file=swagger.json --output-folder=Etdb.UserService.Client.CSharp/src/Etdb.UserService.Client.CSharp/
}

cleanup_dir
publish_service
enable_service
download_swagger_file
setup_git_user
clone_client_repo
#install_node
install_auto_rest
create_csharp_client