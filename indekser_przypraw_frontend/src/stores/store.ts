import { configureStore } from '@reduxjs/toolkit'
import { debugStore } from '@/stores/debugStore.ts'
import { barcodeScannerStore } from '@/stores/barcodeScannerStore.ts'
import { SpiceStore } from '@/stores/SpiceStore.ts'

export const store = configureStore({
  reducer: {
    debug: debugStore.reducer,
    barcode: barcodeScannerStore.reducer,
    spice: SpiceStore.reducer,
  },
})
export type StoreState = ReturnType<typeof store.getState>
