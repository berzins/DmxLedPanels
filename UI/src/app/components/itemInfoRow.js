import React from 'react'


export const ItemInfoRow = ({name, value, style = {}}) => {
    return (
        <div style={style}>
            <span>{name + ' : '}</span>
            <span>{value}</span>
        </div>
    ) 
}