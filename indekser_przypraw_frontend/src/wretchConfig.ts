import wretch from 'wretch'

const apiUrl = import.meta.env.VITE_API_HOST ?? 'http://localhost:5111/'

const spiceApi = wretch(apiUrl + 'api/')

export default spiceApi
