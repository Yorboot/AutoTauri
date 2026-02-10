//This is the main entry point for electron
const { app, BrowserWindow,screen } = require('electron/main')

const createWindow = () => {
    const primaryDisplay = screen.getPrimaryDisplay()
    const { width, height } = primaryDisplay.workAreaSize

    let win = new BrowserWindow({width, height})

    win.loadFile('index.html')
    win.webContents.on('did-finish-load', () => {
        getCars();
    })
}

app.whenReady().then(() => {
    createWindow();

    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) {
            createWindow()
        }
    })
})
async function getCars() {
    try {
        const response = await fetch('http://localhost:8080/all')
        const data = await response.json()
        if(data[0] == null) {
            console.log('No cars found')
            return;
        }else{
            console.log('Cars fetched successfully:')
        }
        const win = BrowserWindow.getAllWindows()[0]
        if (win) {
            win.webContents.send('cars-data', data)
        }
    } catch (error) {
        console.error('Error fetching cars:', error)
    }
}

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit()
    }
})