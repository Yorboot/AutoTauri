const { ipcRenderer } = require('electron')

console.log('Renderer loaded, setting up IPC listener')

ipcRenderer.on('cars-data', (event, cars) => {
    console.log('Received cars data:', cars.length)
    displayCars(cars)
})

function displayCars(cars) {
    console.log('displayCars called')
    const tableBody = document.getElementById('car-overview-body')

    if (!tableBody) {
        console.error('Table body element not found!')
        return
    }

    tableBody.innerHTML = ''

    cars.forEach(car => {
        const row = document.createElement('tr')
        row.innerHTML = `
            <td>${car.id}</td>
            <td>${car.userId}</td>
            <td>${car.licensePlate}</td>
            <td>${car.brand}</td>
            <td>${car.model}</td>
            <td>${car.price}</td>
            <td>${car.mileage}</td>
            <td>${car.seats ?? ''}</td>
            <td>${car.doors ?? ''}</td>
            <td>${car.productionYear ?? ''}</td>
            <td>${car.weight ?? ''}</td>
            <td>${car.color ?? ''}</td>
            <td>${car.image ? `<img src="${car.image}" alt="Car" width="50">` : ''}</td>
            <td>${car.soldAt ?? ''}</td>
            <td>${car.views}</td>
            <td>${new Date(car.createdAt).toLocaleString()}</td>
            <td>${new Date(car.updatedAt).toLocaleString()}</td>
        `
        tableBody.appendChild(row)
    })

    console.log('Table updated with', cars.length, 'cars')
}