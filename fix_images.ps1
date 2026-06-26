$productos = Invoke-RestMethod -Uri "http://localhost:5144/api/Productos" -Method Get

foreach ($producto in $productos) {
    $necesita = $false
    foreach ($img in $producto.imagenes) {
        if ($img.url -like "*xbox*" -or $img.url -like "*nintendo*" -or $img.url -like "*amazon*" -or $img.url -like "*playstation*") {
            $necesita = $true
            if ($producto.nombre -match "Xbox") {
                $img.url = "https://images.unsplash.com/photo-1621259182978-fbf93132d53d?w=800"
            } elseif ($producto.nombre -match "Dual" -or $producto.nombre -match "PlayStation") {
                $img.url = "https://images.unsplash.com/photo-1606144042733-c7823fb8869b?w=800"
            } elseif ($producto.nombre -match "Controller" -or $producto.nombre -match "Mando" -or $producto.nombre -match "Control") {
                $img.url = "https://images.unsplash.com/photo-1592840496694-26d035b52b48?w=800"
            } else {
                $img.url = "https://images.unsplash.com/photo-1550745165-9bc0b252726f?w=800"
            }
        }
    }
    if ($necesita) {
        Write-Host "Updating $($producto.nombre)..."
        $body = $producto | ConvertTo-Json -Depth 5 -Compress
        Invoke-RestMethod -Uri "http://localhost:5144/api/Productos/$($producto.id)" -Method Put -Body $body -ContentType "application/json"
    }
}
Write-Host "Done"
Write-Host "¡Imágenes actualizadas correctamente!"
