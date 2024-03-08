import { ReactElement, useState } from 'react'
import { Collapse } from 'react-collapse'
import ButtonWrapper from '@/components/ButtonWrapper.tsx'
import { join } from '@/utils.ts'

function mapIfArray(
  children: ReactElement[] | ReactElement,
  map: (r: ReactElement) => ReactElement
) {
  if (Array.isArray(children)) return children.map(map)
  return map(children)
}

export default function NestedList({
  children,
  header,
  listItemClassNames,
  listClassNames,
}: NestedListParams) {
  const [isCollapsed, setIsCollapsed] = useState(!header)
  function toggleCollapse() {
    setIsCollapsed((collapsed) => !collapsed)
  }

  return (
    <>
      {header && (
        <ButtonWrapper onClick={toggleCollapse}>{header}</ButtonWrapper>
      )}
      <Collapse isOpened={isCollapsed}>
        <ol className={join(' ', listClassNames ?? '')}>
          {children &&
            mapIfArray(children, (child) => (
              <li className={listItemClassNames}>{child}</li>
            ))}
        </ol>
      </Collapse>
    </>
  )
}

interface NestedListParams {
  header?: ReactElement
  openByDefault?: boolean
  children?: ReactElement[] | ReactElement | null
  listClassNames?: string
  listItemClassNames?: string
}
