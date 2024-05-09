import { SpiceGroup } from '@/types'
import levenshtein from 'fast-levenshtein'

export default function filteredSpiceList(
  spiceGroups: SpiceGroup[],
  filterPhrase: string
) {
  const minLevenshteinDist = spiceGroups.minBy((group) =>
    levenshtein.get(
      group.name.replace(' ', '').slice(0, filterPhrase.length).toLowerCase(),
      filterPhrase.toLowerCase()
    )
  )
  return spiceGroups.filter(
    (spiceGroups) =>
      levenshtein.get(
        spiceGroups.name.slice(0, filterPhrase.length).toLowerCase(),
        filterPhrase.toLowerCase()
      ) == minLevenshteinDist
  )
}
