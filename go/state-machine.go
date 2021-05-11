package main

type Range = [2]int

func CompareRange(a Range, b Range) int {
	if RangeSize(a) < RangeSize(b) {
		return -1
	} else if RangeSize(a) == RangeSize(b) {
		return 0
	} else {
		return 1
	}
}

func RangeSize(r Range) int {
	return r[1] - r[0]
}

type Window struct {
	Left     int
	Right    int
	Size     int
	Saved    Range
	HasSaved bool
}

func NewWindow(size int) *Window {
	return &Window{
		Left:  0,
		Right: -1,
		Size:  size,
	}
}

func (w *Window) CanGo() bool {
	return w.Left < w.Right || w.Right < w.Size || (w.Left == 0 && w.Right == -1)
}

func (window *Window) MoveLeft(step int) bool {
	if window.Left < window.Right {
		window.Left += step
		return true
	}
	return false
}

func (window *Window) MoveRight(step int) bool {
	if window.Right < window.Size {
		window.Right += step
	}
	return window.Right < window.Size
}

func (window *Window) Current() Range {
	return Range{window.Left, window.Right}
}

func (window *Window) Save() {
	window.Saved[0] = window.Left
	window.Saved[1] = window.Right
	window.HasSaved = true
}

func minWindow(s string, t string) string {
	window := NewWindow(len(s))
	m := make(map[byte]int)
	var k int64 = 0
	tk, tm := calcString(t)
	for window.CanGo() {
		if (k & tk) == tk {
			if CompareRange(window.Current(), window.Saved) == -1 || !window.HasSaved {
				window.Save()
			}
			c := s[window.Left]
			key := getKey(c)
			m[c]--
			if m[c] < tm[c] {
				k &^= key
			}
			window.MoveLeft(1)
		} else {
			if window.MoveRight(1) {
				c := s[window.Right]
				key := getKey(c)
				m[c]++
				if m[c] == tm[c] {
					k |= key
				}
			} else {
				break
			}
		}
	}
	if window.HasSaved {
		return s[window.Saved[0] : window.Saved[1]+1]
	} else {
		return ""
	}
}

func calcString(s string) (int64, map[byte]int) {
	m := make(map[byte]int)
	var val int64 = 0
	for i := 0; i < len(s); i++ {
		key := getKey(s[i])
		m[s[i]]++
		val |= key
	}
	return val, m
}

func getKey(c byte) int64 {
	return 1 << (c - 'A')
}
