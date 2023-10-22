import { Compartment } from '@codemirror/state'
import {EditorView, basicSetup} from "codemirror"
import {javascript} from "@codemirror/lang-javascript"
import basicDarkTheme from "cm6-theme-basic-dark"

const themeConfig = new Compartment()

let editor = new EditorView({
  extensions: [basicSetup, javascript(), themeConfig.of(basicDarkTheme)],
  parent: document.getElementById('editor')
})

editor.dispatch({
  changes: {from: 0, insert: "\n\n\n\n\n\n\n"}
})
