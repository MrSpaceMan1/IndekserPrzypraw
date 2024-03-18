import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import { store } from '@/stores/store.ts'
import { Provider } from 'react-redux'
import { TouchContextProvider } from '@/components/TouchRegionContext.tsx'
import { RouterProvider } from 'react-router-dom'
import router from '@/router.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Provider store={store}>
      <TouchContextProvider>
        <RouterProvider router={router} />
      </TouchContextProvider>
    </Provider>
  </React.StrictMode>
)
