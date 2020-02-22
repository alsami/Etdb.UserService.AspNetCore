#!/usr/bin/env bash
function create_image() {
    if [[ -z ${TRAVIS_TAG} ]]; then
        docker build -t etdb.userservice.latest -t ${REGISTRY}/etdb.userservice:latest -f ./src/Etdb.UserService/Dockerfile .
    else 
        docker build -t etdb.userservice.latest -t ${REGISTRY}/etdb.userservice:latest -t ${REGISTRY}/etdb.userservice:${TRAVIS_TAG} -f ./src/Etdb.UserService/Dockerfile .
    fi
}

function publish_image() {
    if [[ -n ${TRAVIS_TAG} ]]; then
        docker push ${REGISTRY}/etdb.userservice:${TRAVIS_TAG}
    fi
    
    docker push ${REGISTRY}/etdb.userservice:latest
}

create_image
publish_image