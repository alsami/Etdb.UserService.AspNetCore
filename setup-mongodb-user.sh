#!/usr/bin/env bash
echo sleeping 5 second before setting up mongodb-user
sleep 5
mongo admin --eval 'db.createUser({user:"'$1'", pwd:"'$2'", roles:["userAdminAnyDatabase", "dbAdminAnyDatabase", "readWriteAnyDatabase"]});'
