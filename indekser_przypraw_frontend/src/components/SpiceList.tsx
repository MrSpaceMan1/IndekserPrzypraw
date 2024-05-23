import { NestedList, NestedListItem } from '@/components'
import { Spice, SpiceGroup } from '@/types'

export default function SpiceList({ spiceGroup }: SpiceListProps) {
  const gramSum = spiceGroup.spices.reduce(
    (acc: number, curr: Spice) => acc + curr.grams,
    0
  )
  const countSum = spiceGroup.spices.length
  const isUnder =
    gramSum < (spiceGroup.minimumGrams ?? 0) ||
    countSum < (spiceGroup.minimumCount ?? 0)

  return (
    <NestedList
      headerClassNames={[isUnder ? 'is-under' : '']}
      key={spiceGroup.spiceGroupId}
      iconClassNames={[isUnder ? 'menu-header-icon-invert' : '']}
      openByDefault={false}
      label={
        <>
          <h2>{spiceGroup.name}</h2>
          <span style={{ marginLeft: 'auto', marginRight: '2rem' }}></span>
          {gramSum < (spiceGroup.minimumGrams ?? 0) ? (
            <h2 className="p-x-1">&lt;{spiceGroup.minimumGrams}g</h2>
          ) : null}
          {countSum < (spiceGroup.minimumCount ?? 0) ? (
            <h2 className="p-x-1">&lt;{spiceGroup.minimumCount} pcs.</h2>
          ) : null}
        </>
      }
    >
      <table className="nested-list-body">
        <thead>
          <tr>
            <th>#</th>
            <th>Data ważności</th>
            <th>Waga</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {spiceGroup.spices.map((spice, index) => (
            <NestedListItem key={spice.spiceId} spice={spice} index={index} />
          ))}
        </tbody>
      </table>
    </NestedList>
  )
}

interface SpiceListProps {
  spiceGroup: SpiceGroup
}
