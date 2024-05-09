import './FetchError.css'

export default function FetchError({ errors }: FetchErrorProps) {
  return (
    <div>
      {Object.entries(errors).map(([key, errors]) => {
        return (
          <div className="error-header">
            <span>{key}</span>
            <ul className="error-list">
              {errors.map((error) => (
                <li>{error}</li>
              ))}
            </ul>
          </div>
        )
      })}
    </div>
  )
}

interface FetchErrorProps {
  errors: { [key: string]: string[] }
}
