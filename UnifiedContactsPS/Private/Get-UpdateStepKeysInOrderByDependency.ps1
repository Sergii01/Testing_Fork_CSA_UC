function  Get-UpdateStepKeysInOrderByDependency {
    param(
        [Parameter(Mandatory = $true)]
        [hashtable]$UpdateDictionary
    )
    
    $keysInOrder = New-Object System.Collections.Generic.List[string]

    $processedSteps = @{}

    $buffer = [array] $UpdateDictionary.Keys

    $index = 0
    $loopDetected = $true

    while ($buffer.Count -gt 0) {

        if ($index -ge $buffer.Count) {
            if ($loopDetected) {
                throw "Loop detected - Buffer was $buffer"
            }
            $index = 0
            $loopDetected = $true
        }
        $keyToCheck = $buffer[$index]

        if ($null -eq $UpdateDictionary[$keyToCheck].Dependencies -or $UpdateDictionary[$keyToCheck].Dependencies.Count -eq 0) {
            $keysInOrder.Add($keyToCheck);
            $buffer = [array] ($buffer | Where-Object { $_ -ne $keyToCheck })
            $processedSteps.Add($keyToCheck, $true)
            $loopDetected = $false
            continue;
        }

        $areAllDependencyProcessed = $true
        foreach ($dependency in $UpdateDictionary[$keyToCheck].Dependencies) {
            if (!$processedSteps.ContainsKey($dependency)) {
                $areAllDependencyProcessed = $false;
                break;
            }
        }

        if ($areAllDependencyProcessed) {
            $keysInOrder.Add($keyToCheck);
            $buffer = [array] ($buffer | Where-Object { $_ -ne $keyToCheck })
            $processedSteps.Add($keyToCheck, $true)
            $loopDetected = $false
            continue;
        }

        $index++
    }

    return $keysInOrder
}