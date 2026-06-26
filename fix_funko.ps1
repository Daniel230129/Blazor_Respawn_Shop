$productos = Invoke-RestMethod -Uri "http://localhost:5144/api/Productos" -Method Get

foreach ($producto in $productos) {
    $necesita = $false
    foreach ($img in $producto.imagenes) {
        if ($producto.nombre -match "Spider-Man") {
            $img.url = "/images/spiderman_funko.png"
            $necesita = $true
        } elseif ($producto.nombre -match "Batman") {
            $img.url = "/images/batman_funko.png"
            $necesita = $true
        }
    }
    if ($necesita) {
        Write-Host "Updating $($producto.nombre)..."
        $body = $producto | ConvertTo-Json -Depth 5 -Compress
        Invoke-RestMethod -Uri "http://localhost:5144/api/Productos/$($producto.id)" -Method Put -Body $body -ContentType "application/json"
    }
}
Write-Host "Hecho!"
