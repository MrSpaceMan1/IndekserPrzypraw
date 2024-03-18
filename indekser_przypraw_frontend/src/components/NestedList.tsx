import { ReactElement, useState } from 'react'
import { Collapse } from 'react-collapse'
import ButtonWrapper from '@/components/ButtonWrapper.tsx'
import { join, mapIfArray } from '@/utils.ts'

export default function NestedList({
  children,
  header,
  label,
  listItemClassNames,
  listClassNames,
}: NestedListParams) {
  const [isCollapsed, setIsCollapsed] = useState(!label)
  function toggleCollapse() {
    setIsCollapsed((collapsed) => !collapsed)
  }

  function mapToListItem(item: ReactElement) {
    return <li className={listItemClassNames}>{item}</li>
  }

  return (
    <>
      {label && <ButtonWrapper onClick={toggleCollapse}>{label}</ButtonWrapper>}
      <Collapse isOpened={isCollapsed}>
        <ol className={join(' ', listClassNames ?? '')}>
          {children && (
            <>
              {header && mapToListItem?.(header)}
              {mapIfArray(children, mapToListItem)}
            </>
          )}
        </ol>
      </Collapse>
    </>
  )
}

interface NestedListParams {
  label?: ReactElement
  header?: ReactElement
  openByDefault?: boolean
  children?: ReactElement[] | ReactElement | null
  listClassNames?: string
  listItemClassNames?: string
}
