import wretch from 'wretch'

// const apiUrl = import.meta.env.VITE_API_HOST ?? 'localhost:8080'
const apiUrl = "192.168.0.45"

const spiceApi = wretch("https://" + apiUrl + '/api/')

export default spiceApi
