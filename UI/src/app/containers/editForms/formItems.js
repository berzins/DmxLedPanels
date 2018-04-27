import React from 'react'

let key = 5000;

// ----

export const selectItem = (title, id, values) => {
    return (
        <div className="col-auto my-1" key={id + (key++)}>
        <label className="mr-sm-2" htmlFor="inlineFormCustomSelect">{title}</label>
        <select className="custom-select mr-sm-2" id={id}>
            {values.map((val, i) => {
                return i === 0 ? 
                <option value={val} key={title + val} defaultValue>{val}</option> : 
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
        <div className="form-group" key={(key++)}>
        <label>{value}</label>
        </div>
    )
}

// ----

export const radioItem = (title, id, checked, callback) => {
    let input = null
    if(checked) {
        input = <input className="form-check-input" type="checkbox" id={id} onChange={() => callback(this)} checked/>
    } else {
        input = <input className="form-check-input" type="checkbox" id={id} onChange={() => callback(this)} />
    }
    return(
        <div className="form-group" key={id * (key++)}>
        <div className="form-check">
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
        <div>
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