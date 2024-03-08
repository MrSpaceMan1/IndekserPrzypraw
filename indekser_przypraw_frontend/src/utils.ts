export function join(joinString: string, ...strings: string[]) {
  let joined = ''
  for (const str of strings) {
    joined += `${str}${joinString}`
  }
  return joined.slice(0, -1)
}
