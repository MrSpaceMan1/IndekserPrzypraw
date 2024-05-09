function checkEan13Validity(barcode: string): boolean {
  const evenNumbers: number[] = []
  const oddNumbers: number[] = []
  for (let i = 1; i < 13; i += 2) {
    evenNumbers.push(parseInt(barcode[i]))
  }
  for (let i = 0; i < 12; i += 2) {
    oddNumbers.push(parseInt(barcode[i]))
  }
  const sum =
    (evenNumbers.reduce((acc, cur) => acc + cur, 0) * 3 +
      oddNumbers.reduce((acc, cur) => acc + cur, 0)) %
    10
  const digit = sum === 0 ? 0 : 10 - sum
  return parseInt(barcode[12]) === digit
}

function checkEan8Validity(barcode: string): boolean {
  const evenNumbers: number[] = []
  const oddNumbers: number[] = []
  for (let i = 0; i < 7; i += 2) {
    evenNumbers.push(parseInt(barcode[i]))
  }
  for (let i = 1; i < 6; i += 2) {
    oddNumbers.push(parseInt(barcode[i]))
  }
  const sum =
    (evenNumbers.reduce((acc, cur) => acc + cur * 3, 0) +
      oddNumbers.reduce((acc, cur) => acc + cur, 0)) %
    10
  const digit = sum === 0 ? 0 : 10 - sum
  return parseInt(barcode[7]) === digit
}

export { checkEan8Validity, checkEan13Validity }
