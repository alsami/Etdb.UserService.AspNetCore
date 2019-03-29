#!/usr/bin/env bash
echo "sleeping 15 second before setting up mongodb-user"
sleep 15
mongo admin --eval 'db.createUser({user:"'$1'", pwd:"'$2'", roles:["userAdminAnyDatabase", "dbAdminAnyDatabase", "readWriteAnyDatabase"]});'
