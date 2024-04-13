import { configureStore } from '@reduxjs/toolkit'
import { debugStore } from '@/stores/debugStore.ts'
import { barcodeScannerStore } from '@/stores/barcodeScannerStore.ts'

export const store = configureStore({
  reducer: {
    debug: debugStore.reducer,
    barcode: barcodeScannerStore.reducer
  },
})
export type StoreState = ReturnType<typeof store.getState>
