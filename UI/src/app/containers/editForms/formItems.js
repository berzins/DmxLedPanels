import React from 'react'

let key = 5000;

// ----

/*
/ onChange
*/
export const selectItem = (title, id, values, index = 0) => {
    const defVal = values[index]
    return (
        <div className="col-auto my-1" key={id + (key++)} id={id + key}>
        <label className="mr-sm-2" htmlFor="inlineFormCustomSelect">{title}</label>
        <select 
        className="custom-select mr-sm-2" 
        id={id}
        >
            {
                values.map((val, i) => {
                return i == index ? 
                <option value={val} key={title + val} selected='selected'>{val}</option> : 
                <option value={val} key={title + val}>{val}</option>
                })
            }
        </select>
        </div>
    )
}

// ----

export const textInputItem = (title, id, value) => {
    return(
        <div className="form-group" key={id + (key++)}>
        <label htmlFor={id}>{title}</label>
        <input type="text" className="form-control" id={id} placeholder={value} defaultValue={value}/>
        </div>
    )
}

export const textItem = (value) => {
    return(
        <div className="form-group" key={value + (key++)}>
        {value}
        </div>
    )
}

// ----

export const radioItem = (title, id, checked) => {
    let input = null
    if(checked) {
        input = <input type="checkbox" data-toggle="toggle" id={id} defaultChecked/>
    } else {
        input = <input type="checkbox" data-toggle="toggle" id={id} />
    }
    return(
        <div className="form-group" key={id + (key++)}>
        <div className="checkbox">
            {input}
        <label className="form-check-label" htmlFor="gridCheck">
            {title}
        </label>
        </div>
    </div>
    )
}

// ----

export const rowItem = (child) => {
    return (
        <div className="col-12" key={key++} >
            <div className="form-row align-items-center">
                {child.map((x) => {return x})}
            </div>    
        </div>
    )
}

// ----

export const contentItem = (child) => {
    return (
        <div key={(key++)}>
            {child.map(x => {return x})}
        </div>
    )
}

// ----

import Modal from 'react-bootstrap4-modal'

export const formModal = (visib, title, submitTxt, cancelTxt, parent, content) => {
    return (
        <Modal visible={visib} onClickBackdrop={() => parent.onClose() }>
            <div className="modal-header">
            <h5 className="modal-title">{title}</h5>
            </div>
            <div className="modal-body">
            <form>
            <div className="form-row align-items-center">
                {content()}
            </div>
            </form>
            </div>
            <div className="modal-footer">
            <button type="button" className="btn btn-secondary" onClick={() => parent.onClose() }>
                {cancelTxt}
            </button>
            <button type="button" className="btn btn-primary" onClick={ () => parent.onSubmit() }>
                {submitTxt}
            </button>
            </div>
        </Modal>
    )
}

// ----

export const errorItem = (visible, error) => {
    if(visible) {
        return(
            <div className="badge badge-danger"  key={(key++)}>{error}</div>
        )
    }
    return(
        <div key={key++}></div>
    )
}

export const buttonItem = (id, title, onClick) => {
    const k = id + title + (key++)

    return (
        <button 
        type="button" 
        className="btn btn-light"
        onClick={() => onClick()}
        key={k}
        >{title}</button>
    )
}

export const labelItem = (text) => {
    return (
        <div 
        key={text + (key++)}
        className="label label-info"
        >{text}</div>
    )
}
