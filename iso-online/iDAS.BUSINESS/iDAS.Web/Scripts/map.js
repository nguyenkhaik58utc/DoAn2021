$(document).ready(function () {

    function convertDate(data) {
        var getdate = parseInt(data.replace("/Date(", "").replace(")/", ""));
        var ConvDate = new Date(getdate);
        var month = parseInt(ConvDate.getMonth()) + 1;
        return ConvDate.getDate() + "/" + month + "/" + ConvDate.getFullYear();
    }


    mapboxgl.accessToken = 'pk.eyJ1Ijoibmd1eWVua2hhaSIsImEiOiJja3E3Z254bnUwM2dsMndxZHpqOGE4aTR6In0.CM0gJMWMaxxT5Kl26oRyQA';
    var map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [105.8342, 21.0278],
        zoom: 11
    });

    var lst = new Array();

    $.get("/Problem/Map/getList", {}, function (res) {

        for (let index in res) {
            var split1 = res[index].OccuredDate.split("(");
            var split2 = split1[1].split(")");
            var dateRegistration = convertDate(split2[0]); 

            let lng = parseFloat(res[index].lng);
            let lat = parseFloat(res[index].lat);

            lst.push({
                type: 'Feature',
                geometry: {
                    type: 'Point',
                    coordinates: [lng, lat]
                },
                properties: {
                    title: res[index].ProblemTypeName,
                    dateCreate: dateRegistration,
                    statusName: res[index].ProblemStatusName,
                    statusId: res[index].StatusID,
                }
            });
        }
    }).done(function () {
        var geojson = {
            type: 'FeatureCollection',
            features: lst
        };

        geojson.features.forEach(function (marker) {
            // create a HTML element for each feature
            var el = document.createElement('div');
            let index = lst.indexOf(marker);
            el.className = 'marker' + index;

            // make a marker for each feature and add it to the map
            new mapboxgl.Marker(el)
                .setLngLat(marker.geometry.coordinates)
                .setPopup(
                    new mapboxgl.Popup({ offset: 25 }) // add popups
                        .setHTML(
                            "<h3 style='color: black'>" +
                            marker.properties.title +
                            "</h3><p style='color:red'> Ngày tạo : " +
                            marker.properties.dateCreate +
                            "</p><p style='color:red'> Trạng thái : " +
                            marker.properties.statusName +
                            "</p>" 
                        )
                )
                .addTo(map);

            switch (parseInt(marker.properties.statusId)) {
                case 2:
                    $('.marker' + index).css("backgroundImage", "url(/Content/pin.png)");
                    $('.marker' + index).css("background-size", "cover");
                    $('.marker' + index).css("width", "30px");
                    $('.marker' + index).css("height", "30px");
                    $('.marker' + index).css("border-radius", "50%");
                    $('.marker' + index).css("cursor", "pointer");
                    break;
                case 1:
                    $('.marker' + index).css("backgroundImage", "url(/Content/placeholder.png)");
                    $('.marker' + index).css("background-size", "cover");
                    $('.marker' + index).css("width", "30px");
                    $('.marker' + index).css("height", "30px");
                    $('.marker' + index).css("border-radius", "50%");
                    $('.marker' + index).css("cursor", "pointer");
                    break;
                case 3:
                    $('.marker' + index).css("backgroundImage", "url(/Content/checked.png)");
                    $('.marker' + index).css("background-size", "cover");
                    $('.marker' + index).css("width", "30px");
                    $('.marker' + index).css("height", "30px");
                    $('.marker' + index).css("border-radius", "50%");
                    $('.marker' + index).css("cursor", "pointer");
                    break;
                case 4:
                    $('.marker' + index).css("backgroundImage", "url(/Content/alarm.png)");
                    $('.marker' + index).css("background-size", "cover");
                    $('.marker' + index).css("width", "30px");
                    $('.marker' + index).css("height", "30px");
                    $('.marker' + index).css("border-radius", "50%");
                    $('.marker' + index).css("cursor", "pointer");
                    break;
            }
        });
    });

    var coordinatesGeocoder = function (query) {
        var matches = query.match(
            /^[ ]*(?:Lat: )?(-?\d+\.?\d*)[, ]+(?:Lng: )?(-?\d+\.?\d*)[ ]*$/i
        );
        if (!matches) {
            return null;
        }

        function coordinateFeature(lng, lat) {
            return {
                center: [lng, lat],
                geometry: {
                    type: 'Point',
                    coordinates: [lng, lat]
                },
                place_name: 'Lat: ' + lat + ' Lng: ' + lng,
                place_type: ['coordinate'],
                properties: {},
                type: 'Feature'
            };
        }

        var coord1 = Number(matches[1]);
        var coord2 = Number(matches[2]);
        var geocodes = [];

        if (coord1 < -90 || coord1 > 90) {
            // must be lng, lat
            geocodes.push(coordinateFeature(coord1, coord2));
        }

        if (coord2 < -90 || coord2 > 90) {
            // must be lat, lng
            geocodes.push(coordinateFeature(coord2, coord1));
        }

        if (geocodes.length === 0) {
            // else could be either lng, lat or lat, lng
            geocodes.push(coordinateFeature(coord1, coord2));
            geocodes.push(coordinateFeature(coord2, coord1));
        }

        return geocodes;
    };

    // Add the control to the map.
    map.addControl(
        new MapboxGeocoder({
            accessToken: mapboxgl.accessToken,
            localGeocoder: coordinatesGeocoder,
            zoom: 15,
            placeholder: 'Tìm kiếm ...',
            mapboxgl: mapboxgl
        })
    );

    //location user
    map.addControl(
        new mapboxgl.GeolocateControl({
            positionOptions: {
                enableHighAccuracy: true
            },
            trackUserLocation: true
        })
    );
    map.addControl(new mapboxgl.NavigationControl());
});