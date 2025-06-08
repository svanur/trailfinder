# TrailFinder.Api

API controllers, middleware

### Health checks

Various health checks are available.

#### **How to Use:**

1. **Basic Health Status:**

``` bash
curl http://your-api/health
```

1.**Readiness Check** (is the app ready to accept traffic?):

``` bash
curl http://your-api/health/ready
```

1.**Liveness Check** (is the app running?):

``` bash
curl http://your-api/health/live
```

Sample Response:

``` json
{
    "status": "Healthy",
    "totalDuration": "00:00:00.2346312",
    "entries": {
        "database": {
            "status": "Healthy",
            "duration": "00:00:00.0040131"
        },
        "postgresql": {
            "status": "Healthy",
            "duration": "00:00:00.0126321"
        }
    }
}
```

#### You can query the health status specifically for GPX storage:

1. Check only storage-related health

``` bash
curl http://your-api/health?tags=storage
```

1. Check only GPX-related health

``` bash
curl http://your-api/health?tags=gpx
```
