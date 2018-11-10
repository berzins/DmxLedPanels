import React from 'react'


export const ItemInfoRow = ({name, value}) => {
    return (
        <div>
            <span>{name + ' : '}</span>
            <span>{value}</span>
        </div>
    ) 
}