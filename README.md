# sermon
Small tool for monitoring other smal tools 

## Api

```
<url>/sermon/info - get comlex info about services
<url>/sermon/allok - get group info about services
```

## Configuration

```
SERMON_CUSTOM_SERVICES - services' names and urls, format name1=whole_url1;name2=whole_url2;...;nameN=whole_utlN, default  
  prehdo=http://localhost:5000/prehdo;  
  datetime=http://localhost:5005/datetime;  
  weather=http://localhost:5010/weather;  
  location=http://localhost:5015/location;  
  system=http://localhost:5020/system;  
  weatherhub=http://localhost:5025/weatherhub;
  covid=http://localhost:5030/covid;
SERMON_CHECK_INTERVAL_S - check interval in seconds, default 5s
```
