var map;
var geocoder;
var currentLat = -37.8136 ;
var currentLong = 144.9631;
var start;
var end; 
var tactileCoords = [];
var directionsService;
var directionsDisplay;
var stepDisplay;
var markerArray = [];
var startLocation = document.getElementById('startAddress');
var endLocation = document.getElementById('address');
var auto = false;


function codeAddress(element) {
    if (!auto) {
        document.getElementById(element.id).value = null;
    } else { auto = false; }
}

var cityBounds = new google.maps.LatLngBounds(
    new google.maps.LatLng(-38.067807865, 144.5148742199),
    new google.maps.LatLng(-37.5687831234, 145.4157531261));

var options = {
    bounds: cityBounds,
    componentRestrictions: { country: "au" }
};

var autocompleteStart = new google.maps.places.Autocomplete(startLocation, options);
autocompleteStart.addListener('place_changed', function () {
    address = autocompleteStart.getPlace();
    auto = true;
    codeAddress(startLocation);
});

var autocompleteEnd = new google.maps.places.Autocomplete(endLocation, options);
autocompleteEnd.addListener('place_changed', function () {
    address = autocompleteEnd.getPlace();
    auto = true;
    codeAddress(endLocation);
});

function initMap() {
    directionsService = new google.maps.DirectionsService();
    directionsDisplay = new google.maps.DirectionsRenderer();
    geocoder = new google.maps.Geocoder();
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 12,
        center: new google.maps.LatLng(currentLat, currentLong),
        mapTypeControl: true,
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
            position: google.maps.ControlPosition.BOTTOM_CENTER
        },
        zoomControl: true,
        zoomControlOptions: {
            position: google.maps.ControlPosition.LEFT_CENTER
        },
        scaleControl: true,
        streetViewControl: true,
        streetViewControlOptions: {
            position: google.maps.ControlPosition.LEFT_BOTTOM
        },
        fullscreenControl: true,
        fullscreenControlOptions: {
            position: google.maps.ControlPosition.RIGHT_BOTTOM
        }
    });
    directionsDisplay.setMap(map);  

    stepDisplay = new google.maps.InfoWindow();
    var marker = new google.maps.Marker({
        map: map,
        position: { lat: currentLat, lng: currentLong }
    });
   
}

function getCurrentLocation() {
    var options = {
        enableHighAccuracy: true,
        timeout: 5000,
        maximumAge: 0
    };

    function success(pos) {
        var currentLocation = pos.coords;
        currentLat = currentLocation.latitude;
        currentLong = currentLocation.longitude;
        var latlng = { lat: parseFloat(currentLocation.latitude), lng: parseFloat(currentLocation.longitude) };
        var geocoder = new google.maps.Geocoder;
        geocoder.geocode({ 'location': latlng }, function (results, status) {
            if (status === 'OK') {
                if (results[0]) {
                    document.getElementById('startAddress').value = results[0].formatted_address;
                    initMap();
                } else {
                    window.alert('No results found');
                }
            } else {
                window.alert('Geocoder failed due to: ' + status);
            }
        });
    }

    function error(err) {
        console.warn(`ERROR(${err.code}): ${err.message}`);
    }

    navigator.geolocation.getCurrentPosition(success, error, options);
}


//geocoding function
function checkRoute() {
    start = "";
    end = null;
    var startAddress = document.getElementById('startAddress').value;
        
    geocoder.geocode({ 'address': startAddress }, function (results, status) {
            if (status === 'OK') {
                map.setCenter(results[0].geometry.location);
                markerArray[0] = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                });
                start = results[0].geometry.location;
            } else {
                getCurrentLocation();
                start = new google.maps.LatLng(currentLat, currentLong);
            }
        });
    var address = document.getElementById('address').value;
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status === 'OK') {
            map.setCenter(results[0].geometry.location);
            markerArray[0] = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location
            });
            end = results[0].geometry.location;         
            calcRoute();
        } else {
            alert('Please enter a Destination!');
        }
    });
}

//directions function
function calcRoute() {  
    for (i = 0; i < markerArray.length; i++) {
        markerArray[i].setMap(null);      
    }
    var request = {
        origin: start,
        destination: end,
        travelMode: 'WALKING',
        region: 'AU'
    };
    directionsService.route(request, function (response, status) {
        if (status === 'OK') {
            directionsDisplay.setDirections(response); 
            showSteps(response);
        }
    });
}

  //rounding function for the geo coordinates
function round(value, decimals) {
    return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}

//Show route navigation function
function showSteps(directionResult) {
    var isWaypoint = 0;
    var tadvisor = document.getElementById('TactileAdvisor');
    // For each step, place a marker, and add the text to the marker's
    // info window. Also attach the marker to an array so we
    // can keep track of it and remove it when calculating new
    // routes.

    var myRoute = directionResult.routes[0].legs[0];

    for (var i = 0; i < myRoute.steps.length; i++) {
        //check route for tactile grounds
        for (j = 0; j < tactileCoords.length; j++) {           
            if (round(myRoute.steps[i].start_point.lat(), 3) === round(tactileCoords[j].lat(), 3) &&
                round(myRoute.steps[i].start_point.lng(), 3) === round(tactileCoords[j].lng(), 3)) {
                tadvisor.innerHTML = '<span><h3 style="background-color: #7FFF00;">Your route includes tactile surfaces!</h3></span>';
                isWaypoint = 1;                                     
            }
        }   
        //Plot waypoints
        var marker = new google.maps.Marker({
            position: myRoute.steps[i].start_point,
            map: map
        });
        attachInstructionText(marker, myRoute.steps[i].instructions);
        markerArray[i] = marker;
    }
    if (isWaypoint === 0) {
        tadvisor.innerHTML = '<span><h3 style="background-color: #e50000;">Our database could not find any tactile surfaces in your route!</h3></span>';
    } 
}

function attachInstructionText(marker, text) {
    google.maps.event.addListener(marker, 'click', function () {
        stepDisplay.setContent(text);
        stepDisplay.open(map, marker);
    });
}

$.ajax({
    url: "https://data.melbourne.vic.gov.au/resource/s3kn-ciyb.json",
    type: "GET",
    data: {
        "$limit": 5000,
        "$$app_token": "m5zAxv4nRKXpqpGjptErnXQXC"
    }
}).done(function (data) {
    initMap();
    for (var i = 0; i < data.length; i++) {
        var coords = data[i].location.coordinates;
        var latlng = new google.maps.LatLng(coords[1], coords[0]);
        tactileCoords.push(latlng);
    }
    });


