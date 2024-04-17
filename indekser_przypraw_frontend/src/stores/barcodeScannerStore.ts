import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import BarcodeInfo from '@/types/BarcodeInfo.ts'

export const barcodeScannerStore = createSlice({
  name: 'barcodeScanner',
  initialState: {
    fetchedBarcodeInfo: {
      barcode: '',
      name: '',
      grams: 0,
    },
  },
  reducers: {
    setFetchedBarcodeInfo: (state, action: PayloadAction<BarcodeInfo>) => {
      console.log(action)
      state.fetchedBarcodeInfo.barcode = action.payload.barcode
      state.fetchedBarcodeInfo.name = action.payload.name
      state.fetchedBarcodeInfo.grams = action.payload.grams
    },
    clearFetchedBarcodeInfo: (state) => {
      state.fetchedBarcodeInfo = {
        barcode: '',
        name: '',
        grams: 0,
      }
    },
  },
})

export const { setFetchedBarcodeInfo, clearFetchedBarcodeInfo } =
  barcodeScannerStore.actions
