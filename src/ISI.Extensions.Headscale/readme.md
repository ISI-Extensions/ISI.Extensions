curl -X "GET" "https://YYYYYYYYYYYYYYYYY/api/v1/apikey" -H "accept: application/json" -H "Authorization: Bearer XXXXXXXXXXXXXX"
curl -X "POST" "https://YYYYYYYYYYYYYYYYY/api/v1/apikey" -H "accept: application/json" -H "Authorization: Bearer XXXXXXXXXXXXXX" -H "Content-Type: application/json" -d "{ \"expiration\": \"2046-05-29T21:22:42.530Z\" }"
curl -X "GET" "https://YYYYYYYYYYYYYYYYY/api/v1/health" -H "accept: application/json" -H "Authorization: Bearer XXXXXXXXXXXXXX"
curl -X "GET" "https://YYYYYYYYYYYYYYYYY/api/v1/policy" -H "accept: application/json" -H "Authorization: Bearer XXXXXXXXXXXXXX"
curl -X "PUT" "https://YYYYYYYYYYYYYYYYY/api/v1/policy" -H "accept: application/json" -H "Authorization: Bearer XXXXXXXXXXXXXX" -H "Content-Type: application/json" -d "{ \"policy\": \"{}\" }"

