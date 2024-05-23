import levenshtein from 'fast-levenshtein'

export default function filteredList<T>(
  items: T[],
  keySelector: (item: T) => string,
  filterPhrase: string
) {
  const minLevenshteinDist = items.minBy((group) =>
    levenshtein.get(
      keySelector(group)
        .replace(' ', '')
        .slice(0, filterPhrase.length)
        .toLowerCase(),
      filterPhrase.toLowerCase()
    )
  )
  return items.filter(
    (item) =>
      levenshtein.get(
        keySelector(item).slice(0, filterPhrase.length).toLowerCase(),
        filterPhrase.toLowerCase()
      ) == minLevenshteinDist
  )
}
