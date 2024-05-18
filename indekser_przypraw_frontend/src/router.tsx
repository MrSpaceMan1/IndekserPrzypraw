import { createBrowserRouter } from 'react-router-dom'
import MainPage from '@/views/MainPage.tsx'
import BarcodeScanner from '@/views/BarcodeScanner.tsx'
import AddSpiceFormPage from '@/views/AddSpiceFormPage.tsx'
import AddDrawerFormPage from '@/views/AddDrawerFormPage.tsx'
import DrawerSettingsPage from '@/views/DrawerSettingsPage.tsx'
import { Router } from '@remix-run/router'
import { App } from '@/App.tsx'
import LoginPage from '@/views/LoginPage.tsx'
import ShoppingListPage from '@/views/ShoppingListPage.tsx'
import SpiceMixPage from '@/views/SpiceMixPage.tsx'
import AddSpiceMixFormPage from '@/views/AddSpiceMixFormPage.tsx'

const router: Router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      { path: '/login', element: <LoginPage /> },
      { path: '/', element: <MainPage /> },
      {
        path: 'barcode-scanner',
        element: <BarcodeScanner />,
      },
      { path: 'add-spice', element: <AddSpiceFormPage /> },
      { path: 'add-drawer', element: <AddDrawerFormPage /> },
      { path: 'drawer/:drawerId/settings', element: <DrawerSettingsPage /> },
      { path: 'shopping-list', element: <ShoppingListPage /> },
      { path: 'spice-mixes', element: <SpiceMixPage /> },
      { path: 'add-spice-mix', element: <AddSpiceMixFormPage /> },
    ],
  },
])
export default router
