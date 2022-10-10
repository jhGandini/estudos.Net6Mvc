﻿using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace App.Extensions;

public static class RazorExtensions
{
    public static string FormataDocumento(this RazorPage page, int tipoPessoa, string documento)
    {
        return tipoPessoa == 1 ? Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00") : Convert.ToUInt64(documento).ToString(@"00\.000\.000\/000-00");
    }
}
