$items = @(
    "PlayStation 5", "Nintendo Switch",
    "DualSense", "Xbox Wireless Controller",
    "Funko Pop Batman", "Funko Pop Spider-Man"
)

foreach ($item in $items) {
    Write-Host "Obteniendo IA para $item..."
    $iaResponse = Invoke-RestMethod -Uri "http://localhost:5144/api/Ia/autocompletar/$([uri]::EscapeDataString($item))" -Method Get
    
    $categoriaId = 1
    if ($iaResponse.categoriaSugerida -match "Consola") { $categoriaId = 2 }
    elseif ($iaResponse.categoriaSugerida -match "Control" -or $iaResponse.categoriaSugerida -match "Accesorio") { $categoriaId = 3 }
    elseif ($iaResponse.categoriaSugerida -match "Funko" -or $iaResponse.categoriaSugerida -match "Figura") { $categoriaId = 4 }
    
    $imagenes = @()
    if ($iaResponse.imagenesSugeridas.Count -gt 0) {
        $imagenes += @{ url = $iaResponse.imagenesSugeridas[0]; esPrincipal = $true }
    }

    $nuevoProducto = @{
        nombre = $iaResponse.nombre
        descripcion = $iaResponse.descripcion
        precio = $iaResponse.precioSugerido
        stock = 10
        esDigital = $iaResponse.esDigital
        categoriaId = $categoriaId
        genero = $iaResponse.genero
        requisitosSistema = $iaResponse.requisitosSistema
        imagenes = $imagenes
    }
    
    $json = $nuevoProducto | ConvertTo-Json -Depth 5
    Write-Host "Insertando producto en DB..."
    Invoke-RestMethod -Uri "http://localhost:5144/api/Productos" -Method Post -Body $json -ContentType "application/json"
    Write-Host "Insertado: $item"
}
