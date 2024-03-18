import { ReactElement } from 'react'

export function join(joinString: string, ...strings: string[]) {
  let joined = ''
  for (const str of strings) {
    joined += `${str}${joinString}`
  }
  return joined.slice(0, -1)
}

export function mapIfArray(
  children: ReactElement[] | ReactElement,
  map: (r: ReactElement) => ReactElement
) {
  if (Array.isArray(children)) return children.map(map)
  return map(children)
}
