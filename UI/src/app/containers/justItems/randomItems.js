import React from 'react'
import File from '../../containers/file'

let key = 6000

export const fileList = (files, parentId = null) => {
    return (
        <div className=" list-group" style={{width: 100+"%"}} key={key++}>
            {files.names.map((f) => {
                return(
                    <File key={f} filename={f.split('.')[0]} parentId={parentId}/>
                )
                })
            }
        </div>
    )
}
