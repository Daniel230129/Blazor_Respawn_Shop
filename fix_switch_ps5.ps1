$productos = Invoke-RestMethod -Uri "http://localhost:5144/api/Productos" -Method Get

foreach ($producto in $productos) {
    $necesita = $false
    foreach ($img in $producto.imagenes) {
        if ($producto.nombre -eq "Nintendo Switch") {
            $img.url = "/images/nintendo_switch.png"
            $necesita = $true
        } elseif ($producto.nombre -match "Dual Sense" -or $producto.nombre -match "DualSense") {
            $img.url = "/images/dual_sense.png"
            $necesita = $true
        }
    }
    if ($necesita) {
        Write-Host "Updating $($producto.nombre)..."
        $producto.categoria = $null
        foreach ($img in $producto.imagenes) {
            $img.producto = $null
        }
        if ($producto.genero -eq "") { $producto.genero = $null }
        if ($producto.requisitosSistema -eq "") { $producto.requisitosSistema = $null }
        
        $body = $producto | ConvertTo-Json -Depth 5 -Compress
        Invoke-RestMethod -Uri "http://localhost:5144/api/Productos/$($producto.id)" -Method Put -Body $body -ContentType "application/json"
    }
}
Write-Host "Completado!"
