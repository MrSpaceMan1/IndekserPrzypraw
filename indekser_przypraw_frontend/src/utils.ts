import { ReactElement } from 'react'

export function join(joinString: string, ...strings: (string | undefined)[]) {
  let joined = ''
  for (const str of strings) {
    if (!str) continue
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

declare global {
  interface SetConstructor {
    constructBy<T, B>(values: Iterable<T>, fn: (item: T) => B): Set<T>
  }
}

Set.constructBy = function constructBy<T, B>(
  values: Iterable<T>,
  fn: (item: T) => B
): Set<T> {
  const set = new Set<T>()
  const keys = new Set<B>()
  for (const item of values) {
    if (!keys.has(fn(item))) {
      set.add(item)
      keys.add(fn(item))
    }
  }
  return set
}

declare global {
  interface Array<T> {
    groupBy<B>(fn: (item: T) => B): { [key: string]: T[] }
    groupMap<B, C>(
      fn: (item: T) => B,
      map: (item: T) => C
    ): { [key: string]: C[] }
    maxBy<B>(fn: (item: T) => B): B
    minBy<B>(fn: (item: T) => B): B
  }
}
Array.prototype.groupBy = function groupBy<T, B>(
  fn: (item: T) => B
): { [key: string]: T[] } {
  const result: Record<string, T[]> = {}
  this.forEach((item: T) => {
    if (result[`${fn(item)}`] === undefined) result[`${fn(item)}`] = [item]
    else result[`${fn(item)}`].push(item)
  })
  return result
}

Array.prototype.groupMap = function groupMap<T, B, C>(
  fn: (item: T) => B,
  map: (item: T) => C
): { [key: string]: C[] } {
  const result: Record<string, C[]> = {}
  this.forEach((item: T) => {
    if (result[`${fn(item)}`] === undefined) result[`${fn(item)}`] = [map(item)]
    else result[`${fn(item)}`].push(map(item))
  })
  return result
}

Array.prototype.maxBy = function maxBy<T, B>(fn: (item: T) => B): B {
  let max = fn(this?.[0])
  for (let i = 1; i < this.length; i++)
    max = fn(this[i]) > max ? fn(this[i]) : max
  return max
}

Array.prototype.minBy = function minBy<T, B>(fn: (item: T) => B): B {
  let min = fn(this?.[0])
  for (let i = 1; i < this.length; i++)
    min = fn(this[i]) < min ? fn(this[i]) : min
  return min
}
