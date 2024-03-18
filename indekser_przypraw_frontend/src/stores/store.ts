import { configureStore } from '@reduxjs/toolkit'
import { debugStore } from '@/stores/debugStore.ts'

export const store = configureStore({
  reducer: {
    debug: debugStore.reducer,
  },
})
export type StoreState = ReturnType<typeof store.getState>
