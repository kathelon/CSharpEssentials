# Changelog

All notable changes to the CSharpEssentials packages are listed here. All packages share one version number.
The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [7.0.0](https://github.com/kathelon/CSharpEssentials/compare/v6.5.2...v7.0.0) (2026-10-10)


### ⚠ BREAKING CHANGES

* **openapi:** CSharpEssentials.AspNetCore.Swashbuckle needs Swashbuckle.AspNetCore [10.2.3,11) and Microsoft.OpenApi [2.7.5,3) on every target. AddSwagger<T>(securityScheme, assembly) is now AddSwagger<T>(securitySchemeName, securityScheme, assembly); pass SecuritySchemes.JwtBearerSchemeName ("Bearer") for the previous behavior. SecuritySchemes.JwtBearerTokenSecurity no longer sets Reference. Custom Swashbuckle filters and MapType factories must move to the Microsoft.OpenApi 2.x API (using Microsoft.OpenApi; IOpenApiSchema; JsonSchemaType; JsonNode).
* **efcore:** offset pagination returns at most 100 rows per page unless a larger cap is given. Pass maxPageSize: 500 (or int.MaxValue for the 5.x behavior) to PaginateAsync/Paginate, or call Normalize(int.MaxValue). The sync Paginate overloads gained a parameter, so assemblies compiled against 5.x must be recompiled. Custom IPaginationRequest types that override Normalize() must also override Normalize(int). See docs/migration/v5-to-v6.md.
* **efcore:** calls to the single-column cursor PaginateAsync now raise CS0618, which fails builds that treat warnings as errors. Migrate to KeysetPaginateAsync (for example .Descending(x => x.CreatedAt).Descending(x => x.Id)) as described in docs/migration/v5-to-v6.md. Old raw cursors are not accepted by KeysetPaginateAsync; clients restart from the first page once.
* **enums:** the CSE enum analyzers (CSE0002-CSE0016) and the generator now run in every project that references any CSharpEssentials package that depends on CSharpEssentials.Enums, including .Validation, .Mediator, .Any, .Maybe, .Entity, .Rules, .These, .Resilience and .GcpSecretManager. New CSE warnings and errors can appear; fix them or set their severity in .editorconfig.
* **enums:** the CSE enum analyzers (CSE0002-CSE0016) and the generator now also run in every project that references CSharpEssentials.Errors, .Results, .AspNetCore.OpenApi or .AspNetCore.Swashbuckle. New CSE diagnostics can appear; fix them or set their severity in .editorconfig.
* **enums:** the CSE enum analyzers (CSE0002-CSE0016) and the generator now run in every project that references CSharpEssentials.Json, .EntityFrameworkCore, .AspNetCore, .Http or the CSharpEssentials meta-package. New CSE warnings can appear (errors under TreatWarningsAsErrors); fix them or set their severity in .editorconfig.
* **swashbuckle:** EnumSchemaFilter takes an IServiceProvider and no longer writes the "Possible values" description.
* **aspnetcore:** hosts that use AddSwagger or the Swagger filters must reference CSharpEssentials.AspNetCore.Swashbuckle.
* **json:** enums without generated metadata are no longer converted by AddEnumConventions, ConditionalStringEnumConverter or a custom CanHandle predicate; use AddEnumConventionsWithReflection. StringEnumNaming, and the EF Core and binding helpers built on it, throw InvalidOperationException for such enums.
* **json:** ConditionalStringEnumConverter.AllowUndefinedValues is removed and undefined values are rejected on read and write. Naming policies other than SnakeCaseLower throw NotSupportedException; set the naming at build time instead.
* **deps:** CSharpEssentials.Resilience and CSharpEssentials.Http no longer bring the Polly package transitively. Consumers relying on Polly v7 APIs (Policy, AsyncRetryPolicy, ...) must reference Polly directly.
* **resilience:** delegates returning a failed Result or Result<T> through a non-generic ResiliencePolicy with WithRetry or WithCircuitBreaker are now retried and counted as circuit breaker failures when the error is transient.
* **efcore:** compile each target framework against its own ef core major
* **gcpsecretmanager:** make options a record and log via ILogger
* **aspnetcore:** resolve result error mapper per request
* **aspnetcore:** default problem details output, result return types and GlobalExceptionHandler mapping changed; see README migration notes.
* **efcore:** enums with acronyms or digits in member names are stored with a different spelling.

### Added

* add CSharpEssentials.Resilience package ([6b42577](https://github.com/kathelon/CSharpEssentials/commit/6b425779656319954f82fadd6ffb5f72a881a84f))
* **aspnetcore:** add AddEnumConventions, enum wire format and route binding through the conventions ([d6264ff](https://github.com/kathelon/CSharpEssentials/commit/d6264ff4cd6817840c458021259179c5243eba52))
* **aspnetcore:** add conditional GET (ETag/Last-Modified, 304) and If-Match preconditions ([f147e91](https://github.com/kathelon/CSharpEssentials/commit/f147e91316ea323aa9b76083f31be890e1f71fb0))
* **aspnetcore:** add conditional GET (ETag/Last-Modified, 304) and If-Match preconditions ([5f1e229](https://github.com/kathelon/CSharpEssentials/commit/5f1e22935277b00d7d53849f80738a536679fe4b)), closes [#78](https://github.com/kathelon/CSharpEssentials/issues/78)
* **aspnetcore:** add enum binding error factory and nested query models ([3cdc1a6](https://github.com/kathelon/CSharpEssentials/commit/3cdc1a68fcfb6793d0bea55a808aa3fb63aa6613))
* **aspnetcore:** add enum query and route binding ([a689528](https://github.com/kathelon/CSharpEssentials/commit/a689528b3f1921fa0e23a2cf49f5d0987d3f37b0))
* **aspnetcore:** add Idempotency-Key middleware with pluggable IIdempotencyStore ([508f985](https://github.com/kathelon/CSharpEssentials/commit/508f9859bceef6dfbe6f902a9d5ea1b2a071fcdf))
* **aspnetcore:** add Idempotency-Key middleware with pluggable IIdempotencyStore ([60caf33](https://github.com/kathelon/CSharpEssentials/commit/60caf33c7ee68f60e7082ffd447d85d8e3224832)), closes [#79](https://github.com/kathelon/CSharpEssentials/issues/79)
* **aspnetcore:** add invalid model state problem response ([b7b1c3a](https://github.com/kathelon/CSharpEssentials/commit/b7b1c3a4859815e9abdafe299d7d266eee5f5d7c))
* **aspnetcore:** add validation endpoint filter ([05256e2](https://github.com/kathelon/CSharpEssentials/commit/05256e29194c673a043060a74645071fb21c7b21)), closes [#58](https://github.com/kathelon/CSharpEssentials/issues/58)
* **aspnetcore:** make problem details configurable and secure by default ([5d3386e](https://github.com/kathelon/CSharpEssentials/commit/5d3386ef0595d298015504ed1616d21cb4482c80))
* **aspnetcore:** move the Swagger support to CSharpEssentials.AspNetCore.Swashbuckle ([a9ab837](https://github.com/kathelon/CSharpEssentials/commit/a9ab837e433dc419ef1a5042ebbc535642ca17bd))
* **collections:** batch APIs for Result, Maybe, and Any (resolved conflicts) ([#23](https://github.com/kathelon/CSharpEssentials/issues/23)) ([e7e06f1](https://github.com/kathelon/CSharpEssentials/commit/e7e06f153d540835671ea0f6042dac7933140114))
* **core:** add IsBetween and IsBetweenExclusive range checks ([79da7dd](https://github.com/kathelon/CSharpEssentials/commit/79da7dddbabd35e3a06d08d0626b4579a3115518))
* **di:** add cse2006 captive dependency diagnostic ([0a73875](https://github.com/kathelon/CSharpEssentials/commit/0a7387514fe426924f4f6ee8b682a2b42fdac575)), closes [#58](https://github.com/kathelon/CSharpEssentials/issues/58)
* **di:** add net8.0 target ([3a0b9df](https://github.com/kathelon/CSharpEssentials/commit/3a0b9dfa4fa4f6159473ac88dccd5d8ff8c7263c))
* **di:** add registration attributes and runtime decoration ([239215b](https://github.com/kathelon/CSharpEssentials/commit/239215b333f0cef4ff076ac32eab9c4faa215b83)), closes [#55](https://github.com/kathelon/CSharpEssentials/issues/55)
* **di:** add service registration generator and analyzer ([8d753ed](https://github.com/kathelon/CSharpEssentials/commit/8d753ed6ae74b29e443774c261bf108d3ad3508e)), closes [#56](https://github.com/kathelon/CSharpEssentials/issues/56)
* **docs:** add Validation package card to GitHub Pages landing page ([7eec7b0](https://github.com/kathelon/CSharpEssentials/commit/7eec7b0f104292a3d23aeb7e97a2365e9237fcd6))
* **efcore:** add composite keyset pagination with opaque cursors ([a505653](https://github.com/kathelon/CSharpEssentials/commit/a50565399c15c55908d18b5372663a65591f8ff7))
* **efcore:** add composite keyset pagination with opaque cursors ([3835744](https://github.com/kathelon/CSharpEssentials/commit/38357441eb23b42146b9fe2a1eea46dafab38e9c)), closes [#87](https://github.com/kathelon/CSharpEssentials/issues/87)
* **efcore:** add EF Core 10 named query filter helpers and IgnoreQueryFilters analyzer ([1fb0164](https://github.com/kathelon/CSharpEssentials/commit/1fb0164dbb0a29adb2a6f7b1a6af4d5a244c6118))
* **efcore:** add EF Core 10 named query filter helpers and IgnoreQueryFilters analyzer ([3e91857](https://github.com/kathelon/CSharpEssentials/commit/3e91857d981ee0be19db80b277769f263fdac6d0)), closes [#77](https://github.com/kathelon/CSharpEssentials/issues/77)
* **efcore:** add enum column migration helpers and data audit ([19ce4e6](https://github.com/kathelon/CSharpEssentials/commit/19ce4e6c771226df2bc5565fcfbe6f5404888927))
* **efcore:** add IDbErrorTranslator with SQLSTATE default ([7755501](https://github.com/kathelon/CSharpEssentials/commit/77555018cf4006ce51fb1e8a65a3686ddd1b6c86))
* **efcore:** add IDbErrorTranslator with SQLSTATE default ([ad06306](https://github.com/kathelon/CSharpEssentials/commit/ad0630634c0df084668f476470b43670f21e8003)), closes [#74](https://github.com/kathelon/CSharpEssentials/issues/74)
* **efcore:** add nullable Maybe&lt;T&gt;? column mapping ([a42be18](https://github.com/kathelon/CSharpEssentials/commit/a42be18177869a51bdf56101fe10e5489aba427c)), closes [#106](https://github.com/kathelon/CSharpEssentials/issues/106)
* **efcore:** add opt-in max limit to pagination request normalization ([68c2f18](https://github.com/kathelon/CSharpEssentials/commit/68c2f18e3adc4ec6acf01eb9f48ad4f13f647614))
* **efcore:** add opt-in max limit to pagination request normalization ([4a77d5b](https://github.com/kathelon/CSharpEssentials/commit/4a77d5b445342039cf4711d8e923cc512cc0efcc)), closes [#92](https://github.com/kathelon/CSharpEssentials/issues/92)
* **efcore:** add soft-delete batch, paging overloads and base context hooks ([ce6b851](https://github.com/kathelon/CSharpEssentials/commit/ce6b8518dc225e5c878171caf049d0332067a307)), closes [#7](https://github.com/kathelon/CSharpEssentials/issues/7)
* **efcore:** cap offset page size at 100 by default ([74badad](https://github.com/kathelon/CSharpEssentials/commit/74badad9db80607a798a41c282e064a2d3578985))
* **efcore:** map Maybe&lt;T&gt;? properties to nullable columns ([ce782e2](https://github.com/kathelon/CSharpEssentials/commit/ce782e229714c54f94e04ceb091c9dba40a9bcaf))
* **efcore:** store enums as wire names or integers with check constraints ([7cb982f](https://github.com/kathelon/CSharpEssentials/commit/7cb982f2e2fe8e3a1522631e923cf95092713db1)), closes [#64](https://github.com/kathelon/CSharpEssentials/issues/64)
* **efcore:** store enums with the json naming policy ([bfb4484](https://github.com/kathelon/CSharpEssentials/commit/bfb44849739c2f12e68e6a8fdc01dc3e3e3db41f))
* **endpoints,aspnetcore:** add reflection fallback and versioned group ([e061339](https://github.com/kathelon/CSharpEssentials/commit/e0613399c6e1c21109a179bc9ead97b5b7396166)), closes [#54](https://github.com/kathelon/CSharpEssentials/issues/54)
* **endpoints,di:** add code fixes for cse1004 and cse2003 ([0194917](https://github.com/kathelon/CSharpEssentials/commit/0194917b9dce575d8723035a932257d3055c9f08)), closes [#58](https://github.com/kathelon/CSharpEssentials/issues/58)
* **endpoints:** add analyzer diagnostics cse1001-cse1004 and cse1006 ([bb832ea](https://github.com/kathelon/CSharpEssentials/commit/bb832eaa6d18905ac479306a8b7a2a9d5bc540f7)), closes [#53](https://github.com/kathelon/CSharpEssentials/issues/53)
* **endpoints:** add authorization shortcuts ([8695c00](https://github.com/kathelon/CSharpEssentials/commit/8695c002d6ad742344d4814cd82adba5842bf4d6)), closes [#58](https://github.com/kathelon/CSharpEssentials/issues/58)
* **endpoints:** add cse1005 duplicate route diagnostic ([c675f51](https://github.com/kathelon/CSharpEssentials/commit/c675f5112204739cf65507eb722c5c0149dfcad9)), closes [#58](https://github.com/kathelon/CSharpEssentials/issues/58)
* **endpoints:** add endpoint contracts and runtime ([8db0cbb](https://github.com/kathelon/CSharpEssentials/commit/8db0cbbdd437f527b2195b8bde8bedac9ccf015b)), closes [#51](https://github.com/kathelon/CSharpEssentials/issues/51)
* **endpoints:** add RouteOf typed route helper ([2f9443e](https://github.com/kathelon/CSharpEssentials/commit/2f9443e2405f32aaefc458cfeb9e594fbbef649f)), closes [#58](https://github.com/kathelon/CSharpEssentials/issues/58)
* **endpoints:** generate endpoint registry and aggregate ([37446b1](https://github.com/kathelon/CSharpEssentials/commit/37446b1e7fc8806f38c107828a0c715e7f7ed462)), closes [#52](https://github.com/kathelon/CSharpEssentials/issues/52)
* **endpoints:** reserve explicit endpoint names at build time and add cse1010 ([edca751](https://github.com/kathelon/CSharpEssentials/commit/edca75159e4bd04689f6b9041118a0328ec2c833))
* **enums:** add analyzers for wire names, aliases, fallbacks and flags ([f0fe515](https://github.com/kathelon/CSharpEssentials/commit/f0fe5153dea20a6392b324c8137efb82e69bf2c8))
* **enums:** add code fixes for explicit values and none member ([5dc278b](https://github.com/kathelon/CSharpEssentials/commit/5dc278b2cb5d42e3032c3573811e57feaf21124f))
* **enums:** add enum metadata, conventions, parser and formatter ([498e6d4](https://github.com/kathelon/CSharpEssentials/commit/498e6d40fb94007164919665baedc9a92039dca4)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **enums:** add non-generic EnumValueFormatter overloads ([5d78319](https://github.com/kathelon/CSharpEssentials/commit/5d783192e47f5a157d2f621420fd1343f48e3851)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **enums:** generate enum metadata, registration and wire-name helpers ([ce6266c](https://github.com/kathelon/CSharpEssentials/commit/ce6266cb903422e927a30f41e2b085eaffb7f5bc)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **enums:** report AlterColumn next to ConvertEnumColumn for the same column (CSE0014) ([b74f24d](https://github.com/kathelon/CSharpEssentials/commit/b74f24dbe76a27d3cd4b86eb8a08c85805682e96))
* **enums:** report nested string enums with cse0001 ([6cc6767](https://github.com/kathelon/CSharpEssentials/commit/6cc6767dc4306af15131fc54dd768b995724060a))
* **enums:** warn with CSE0015 when a [StringEnum] enum gets no generated metadata ([2cef288](https://github.com/kathelon/CSharpEssentials/commit/2cef2884e0f001f1f1176eae7def2601a15daadd)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **http:** add SSRF guard for outbound HttpClient requests ([37329b2](https://github.com/kathelon/CSharpEssentials/commit/37329b2c636cf0d5dfe93800a848e97217fce6a3))
* **http:** add SSRF guard for outbound HttpClient requests ([c97b0cc](https://github.com/kathelon/CSharpEssentials/commit/c97b0cc1a7df6c48d43ec188053ecdf7afe8cdf6)), closes [#80](https://github.com/kathelon/CSharpEssentials/issues/80)
* **http:** format enums in query and route through EnumValueFormatter ([22322b2](https://github.com/kathelon/CSharpEssentials/commit/22322b2efa70ca942fbc207e152cc78af1961ed6))
* implement PARITY-PLAN items — These package, Maybe/Result extensions, RuleEngine.FromPredicate, FakeDateTimeProvider ([9deac8d](https://github.com/kathelon/CSharpEssentials/commit/9deac8de1422c80f7b3b2e683dfc936ab6915482))
* **json:** add EnumConverterFactory and AddEnumConventions ([4b0c850](https://github.com/kathelon/CSharpEssentials/commit/4b0c850772e66c3eda08f4de806a6d036e3fc4f1)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **json:** add JsonElement.ToClrObject ([c3d3f26](https://github.com/kathelon/CSharpEssentials/commit/c3d3f267836bb5665d8fd7e07d68742e297c16e5))
* **json:** add strict undefined enum value mode ([3803d10](https://github.com/kathelon/CSharpEssentials/commit/3803d10fb6e86df2d5f61c7f5a7c0eded1a9120b))
* **json:** add StringEnumNaming as single source for enum names ([d315c12](https://github.com/kathelon/CSharpEssentials/commit/d315c124796fab794ce62db3ae9089f6666ab0e2))
* **json:** make reflection enum metadata an explicit opt-in and fail loud for [StringEnum] without metadata ([7d36a10](https://github.com/kathelon/CSharpEssentials/commit/7d36a10ebe48c2efe4da9bad3db8e0e117ec4c63)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **maybe:** add Async-suffixed names for Execute, ExecuteNoValue, Or, Match, ToMaybeResult, From ([2ec0851](https://github.com/kathelon/CSharpEssentials/commit/2ec0851325a24f46694a6dc56440c9e406997a0b)), closes [#121](https://github.com/kathelon/CSharpEssentials/issues/121)
* **maybe:** add Flatten for Maybe&lt;T&gt;? ([8765bc9](https://github.com/kathelon/CSharpEssentials/commit/8765bc95d035dbf122b5bbf4fd020e3111b9bbde))
* **maybe:** Async-suffixed names for Execute, ExecuteNoValue, Or, Match, ToMaybeResult, From ([49b9a0b](https://github.com/kathelon/CSharpEssentials/commit/49b9a0b4a57ef6270b48215eebaa6fb83e842da1))
* **mediator:** add ExceptionHandlingBehavior pipeline ([#38](https://github.com/kathelon/CSharpEssentials/issues/38)) ([55547b2](https://github.com/kathelon/CSharpEssentials/commit/55547b25f0847d14f6628e94e3a79d29c2de5dd7))
* **mediator:** add IResourceLock and LockBehavior ([414888d](https://github.com/kathelon/CSharpEssentials/commit/414888d40c9d5e7ebc9abd6cf29982513e958825))
* **mediator:** add IResourceLock and LockBehavior ([2d08bfb](https://github.com/kathelon/CSharpEssentials/commit/2d08bfb17b00873314acc48a60a89d924c71c9f3)), closes [#76](https://github.com/kathelon/CSharpEssentials/issues/76)
* **mediator:** add ITransactionRunner and TransactionBehavior ([7335fe3](https://github.com/kathelon/CSharpEssentials/commit/7335fe35b9f7e3630da01117aa0f33a826e8725f))
* **mediator:** add ITransactionRunner and TransactionBehavior ([105a3ac](https://github.com/kathelon/CSharpEssentials/commit/105a3ac6f74fb8c53ebdbd43d7a27d3c1f227245)), closes [#72](https://github.com/kathelon/CSharpEssentials/issues/72)
* **mediator:** add validation modes and failure observers ([c4806e1](https://github.com/kathelon/CSharpEssentials/commit/c4806e168344e387cf890c546363c0885ff3eef3))
* **mediator:** add validation modes and failure observers ([ecf9873](https://github.com/kathelon/CSharpEssentials/commit/ecf98739cfe431413a26ccaec1b22dba6fcd7eed)), closes [#73](https://github.com/kathelon/CSharpEssentials/issues/73)
* **mediator:** surface validation errors based on TResponse type ([c0456fb](https://github.com/kathelon/CSharpEssentials/commit/c0456fb8a3de5646409c17b3c78c2d5fe407b3a4))
* **meta:** add CSharpEssentials.These to meta package ([b0f0937](https://github.com/kathelon/CSharpEssentials/commit/b0f0937026ba2a1bd6b512794f734bccb43f4e65))
* **openapi:** describe optional route parameters in Microsoft.AspNetCore.OpenApi documents ([33855a3](https://github.com/kathelon/CSharpEssentials/commit/33855a3860a6efa0812cb3865752b04d0f2c0b24))
* **openapi:** emit spec-valid optional route parameters by splitting paths ([f5ff643](https://github.com/kathelon/CSharpEssentials/commit/f5ff643267e12e53587b9352afcc6c2140fb8c41))
* **openapi:** emit spec-valid optional route parameters by splitting paths ([e32afea](https://github.com/kathelon/CSharpEssentials/commit/e32afea0328900e37e7355cac9a21c9cc6f09a77)), closes [#101](https://github.com/kathelon/CSharpEssentials/issues/101)
* **openapi:** optional route parameters in Microsoft.AspNetCore.OpenApi, fallback warning, goldens ([dd71944](https://github.com/kathelon/CSharpEssentials/commit/dd7194497c14569ce06ce590ab291c114ec97647))
* **openapi:** require Swashbuckle 10 and Microsoft.OpenApi 2 on all targets ([ca352cf](https://github.com/kathelon/CSharpEssentials/commit/ca352cf0675b2105adbbae6adb0fc4dc2fdd1d91)), closes [#69](https://github.com/kathelon/CSharpEssentials/issues/69)
* **openapi:** warn once per enum when a document mixes string and number formats ([df183b7](https://github.com/kathelon/CSharpEssentials/commit/df183b7eae95a20f72dc5db9ccfff3b89a0c0c91))
* **openapi:** warn when SplitPaths keeps an operation on one path ([593de9b](https://github.com/kathelon/CSharpEssentials/commit/593de9b53fdefc62c337a2cad26b613d1ac07ce7))
* **resilience:** add CSharpEssentials.Resilience package with modular policy system ([e6f4a85](https://github.com/kathelon/CSharpEssentials/commit/e6f4a85688d7a3ca64375b6226d8d9e2f262a496))
* **resilience:** add CSharpEssentials.Resilience package with modular policy system ([e6f4a85](https://github.com/kathelon/CSharpEssentials/commit/e6f4a85688d7a3ca64375b6226d8d9e2f262a496))
* **resilience:** add retryiffailed overloads with a shouldretry predicate ([5df7bf2](https://github.com/kathelon/CSharpEssentials/commit/5df7bf2678e6af39ff5e71c5c76330da7874a130))
* **resilience:** retry failed results in non-generic policy ([8df8735](https://github.com/kathelon/CSharpEssentials/commit/8df8735e7340165db6e359ce98b27ccdd1d9bda1))
* **results:** add Async-suffixed names for task-returning members ([e024eb6](https://github.com/kathelon/CSharpEssentials/commit/e024eb6d108628942fcc50f573a74e97e5294c1a))
* **results:** add Async-suffixed names for task-returning members ([18e8b75](https://github.com/kathelon/CSharpEssentials/commit/18e8b75fda126cf3b8cc88b8b93fa26ff352abf0)), closes [#108](https://github.com/kathelon/CSharpEssentials/issues/108)
* **results:** add MapErrorAsync overloads ([e0e78e2](https://github.com/kathelon/CSharpEssentials/commit/e0e78e2982b2f2425ca30636f0053b0539e20d72))
* **results:** add sequential TraverseAsync and SequenceAsync ([07c664c](https://github.com/kathelon/CSharpEssentials/commit/07c664cf176c847ed4668e5bb5876ea78a52bdd4))
* **results:** add sequential TraverseAsync and SequenceAsync ([dc9fbcc](https://github.com/kathelon/CSharpEssentials/commit/dc9fbcc22666e08354927ae857fc556c95685bd2)), closes [#108](https://github.com/kathelon/CSharpEssentials/issues/108)
* **results:** add ValueTask twins for remaining async operations ([5f1a0db](https://github.com/kathelon/CSharpEssentials/commit/5f1a0dbcfaec2a2ab17a34b5e3c94688df9ea6cb)), closes [#122](https://github.com/kathelon/CSharpEssentials/issues/122)
* **results:** fill flavour-matched async matrix ([8ad41e5](https://github.com/kathelon/CSharpEssentials/commit/8ad41e550fe750077ca226968a63581a97674548))
* **results:** fill flavour-matched async matrix ([e8f4f49](https://github.com/kathelon/CSharpEssentials/commit/e8f4f49ab413ba9cdf6a5f06e5fe455b6a6d29a3)), closes [#108](https://github.com/kathelon/CSharpEssentials/issues/108)
* **results:** ValueTask twins for remaining async operations (6.5) ([ddee9fd](https://github.com/kathelon/CSharpEssentials/commit/ddee9fdedf9df3d395d9c8fad75a3e7b93da87c8))
* **rules:** add async RuleEngine.EvaluateAsync that awaits async rules ([e66f8bc](https://github.com/kathelon/CSharpEssentials/commit/e66f8bcb9c76c594d4bca7d5c8243f00d57279e4))
* **rules:** add RuleEngine.EvaluateAsync that awaits async rules ([d235b5d](https://github.com/kathelon/CSharpEssentials/commit/d235b5d11c33bc30c388878219efa809289d932d))
* **swashbuckle:** describe enums by the enum conventions ([048d05d](https://github.com/kathelon/CSharpEssentials/commit/048d05d4e6d6151505e9153bac4cb130782412d6)), closes [#63](https://github.com/kathelon/CSharpEssentials/issues/63)
* **these:** JSON serialization, ToResult semantics fix, test coverage ([#40](https://github.com/kathelon/CSharpEssentials/issues/40)) ([05de694](https://github.com/kathelon/CSharpEssentials/commit/05de6943d08454fe249112ef17e7e6542f8c24b6))
* **time:** add next/previous day-of-week and GetAge extensions ([6563c7a](https://github.com/kathelon/CSharpEssentials/commit/6563c7ab7ad5509046a944813b63ed679cf706c5))
* **validation:** add CSharpEssentials.Validation and migrate mediator ([8611cfa](https://github.com/kathelon/CSharpEssentials/commit/8611cfad6d3a6e7e89976d6ab34f3d111b57486f))
* **validation:** add enum rules that reuse the binding error text ([b36bafc](https://github.com/kathelon/CSharpEssentials/commit/b36bafce4fa575d0ea97d349187ff66cc8a4b187))
* **validation:** add Examples.Validation console project with full API showcase ([2fe0708](https://github.com/kathelon/CSharpEssentials/commit/2fe070874a473b26913e5cba02085b9a9914180c))
* **validation:** add FluentValidation benchmark suite with 23 scenarios ([e29a68c](https://github.com/kathelon/CSharpEssentials/commit/e29a68caaa956385733e58a7ecff1e1fc1dd85c1))
* **validation:** add railway validation bindings on Result, Task, and ValueTask ([#18](https://github.com/kathelon/CSharpEssentials/issues/18)) ([ce36d5e](https://github.com/kathelon/CSharpEssentials/commit/ce36d5e9bec3e1f590b91917211f963b7214e8e8))
* **validation:** add ValueTask-based MustAsync overload ([b040a6e](https://github.com/kathelon/CSharpEssentials/commit/b040a6e3533ce6e7e17c530c2541bc3ada9d1337))


### Fixed

* add ConfigureAwait(false) across library code and enforce CA2007 ([903e267](https://github.com/kathelon/CSharpEssentials/commit/903e2670e6f35a386d873a232b211c932e24ffa7))
* add ConfigureAwait(false) across library code and enforce CA2007 ([c54f124](https://github.com/kathelon/CSharpEssentials/commit/c54f1240c0a1bcb653f2ff23f17e3c0240126c7a)), closes [#123](https://github.com/kathelon/CSharpEssentials/issues/123)
* **aspnetcore:** bind whitespace-only enum values to null for nullable targets ([388e3b2](https://github.com/kathelon/CSharpEssentials/commit/388e3b2ccbabb9347fd670498342d24e4c04dc5e))
* **aspnetcore:** fall back to trace identifier for w3c trace id ([f521284](https://github.com/kathelon/CSharpEssentials/commit/f521284450fe6e568af88b13d121993280412b11))
* **aspnetcore:** keep the host output formatters when applying an enum wire format override ([e5a8196](https://github.com/kathelon/CSharpEssentials/commit/e5a8196292cef324b1c2236ab27e3a4257f3ee09))
* **aspnetcore:** post-configure invalid model state response factory ([99d9f6d](https://github.com/kathelon/CSharpEssentials/commit/99d9f6de7821ceb245942bf5789a18e122b5d11f))
* **aspnetcore:** resolve result error mapper per request ([2d27ac2](https://github.com/kathelon/CSharpEssentials/commit/2d27ac26457b5e11bfcdf30f8e82568aeaa10718))
* **aspnetcore:** trim surrounding whitespace of enum binding values ([9483220](https://github.com/kathelon/CSharpEssentials/commit/94832205c629d7587a87234b69ee76e66cd22af6))
* **aspnetcore:** validate only the first value source of an unannotated MVC enum parameter ([77f32aa](https://github.com/kathelon/CSharpEssentials/commit/77f32aadf2dd3bb973a61cb18a191c9c3a8cf005))
* **build:** drop redundant null-forgiving operators in benchmarks ([57beb0b](https://github.com/kathelon/CSharpEssentials/commit/57beb0b67919820dafa126729273d0ba02074f29))
* **build:** resolve new sonar analyzer warnings ([39dff93](https://github.com/kathelon/CSharpEssentials/commit/39dff93787d64444e51d95c6d31f434ae6412a28))
* **core:** bound stackalloc and validate guider input ([#142](https://github.com/kathelon/CSharpEssentials/issues/142)) ([2eddfba](https://github.com/kathelon/CSharpEssentials/commit/2eddfba5a4220efc54aa46d8b734517bfe686cc8)), closes [#136](https://github.com/kathelon/CSharpEssentials/issues/136)
* **di,endpoints:** include host registry in name collision check ([e8df9d2](https://github.com/kathelon/CSharpEssentials/commit/e8df9d2c0a1f5b7706672836e1dc96de46c43088))
* **di:** apply decorator order across assemblies ([054fca1](https://github.com/kathelon/CSharpEssentials/commit/054fca1ec07c3292203382853a28e5dbb7b08ba7))
* **di:** emit c# 8 compatible generated code ([cbdf895](https://github.com/kathelon/CSharpEssentials/commit/cbdf895fa71f749b9730bc3a3ebd478516cd11e4))
* **di:** hide decorated inner registration behind a marker type ([be49ee1](https://github.com/kathelon/CSharpEssentials/commit/be49ee1bb3944e02c4e63b12176f9324a06a4d92))
* **di:** inherit service key for keyed decorator dependencies ([2208d89](https://github.com/kathelon/CSharpEssentials/commit/2208d8922669bfb023b14ee5e60122c81bca8071))
* **di:** make generated service registration idempotent ([96e8f55](https://github.com/kathelon/CSharpEssentials/commit/96e8f55de3290812cfa3676b38ae7e9c0c4b19ed))
* **di:** mark assembly registered only after registrations succeed ([62fbcc4](https://github.com/kathelon/CSharpEssentials/commit/62fbcc4dd07b1f6309e8bf5f5210b22fbffe2dc5))
* **di:** register record services in the generator ([6a32dda](https://github.com/kathelon/CSharpEssentials/commit/6a32ddaf2d1ce7ef6da378559347042631a184b6))
* **di:** report throw strategy only for later duplicates ([13a1378](https://github.com/kathelon/CSharpEssentials/commit/13a1378b072c3dae5bda5718f822781ea58b5988))
* **di:** resolve inherited service keys in cse2006 ([dbe6c85](https://github.com/kathelon/CSharpEssentials/commit/dbe6c85457082f72f40cd532a9d3f1f76fb83a63))
* **di:** skip colliding registries in the aggregate and report cse2009 ([980c99f](https://github.com/kathelon/CSharpEssentials/commit/980c99f160ca0aae434e8365456876436c846cd3))
* **docs:** remove leftover merge conflict markers from README ([#29](https://github.com/kathelon/CSharpEssentials/issues/29)) ([19bdf8d](https://github.com/kathelon/CSharpEssentials/commit/19bdf8dd2e4b9af87615f51e43b7c61f29030cb6))
* **efcore:** compare text[] flags columns as enum values on PostgreSQL ([d912a28](https://github.com/kathelon/CSharpEssentials/commit/d912a28f8017e130b5a2fce83d2b4fd840343793)), closes [#64](https://github.com/kathelon/CSharpEssentials/issues/64)
* **efcore:** dispose context scope and keep after-save events on failed save ([8f0f1ca](https://github.com/kathelon/CSharpEssentials/commit/8f0f1ca3dc26f18902ff5dc2e00ef6d086539494))
* **efcore:** keep SQLite check constraints enforced when an enum conversion fails ([fc971dd](https://github.com/kathelon/CSharpEssentials/commit/fc971dddb3a7257f292e02695c942a62b7676867))
* **efcore:** map Maybe&lt;T&gt;? in complex types and document convention limits ([10ca7ef](https://github.com/kathelon/CSharpEssentials/commit/10ca7efe1de4fcbbe2a7236000e430bdd1886531))
* **efcore:** match a missing migration schema against any schema in CSE0014 ([23a32c8](https://github.com/kathelon/CSharpEssentials/commit/23a32c84569973b1e363ce1c5b62340501f5cf6e))
* **efcore:** name the PostgreSQL flags conversion column from a short hash of the column name ([5d08837](https://github.com/kathelon/CSharpEssentials/commit/5d088371208af1d6382578d7992f472ae059daa5))
* **efcore:** prevent int overflow in pagination skip and take ([07bfc4a](https://github.com/kathelon/CSharpEssentials/commit/07bfc4a879023499bfe7be343534b9c38d39ee26))
* **efcore:** resolve ConfigureEnumConventions(null) ambiguity with OverloadResolutionPriority ([905ba3c](https://github.com/kathelon/CSharpEssentials/commit/905ba3c5475aefc75a300df7afdac1a1f87349d4))
* **efcore:** split SQLite flags text with a recursive CTE instead of json_each ([2a9fb76](https://github.com/kathelon/CSharpEssentials/commit/2a9fb766307980dfc2baa84060e69f0618b82454))
* **efcore:** throw when an obsolete CanConvert predicate selects an enum without metadata ([ec960da](https://github.com/kathelon/CSharpEssentials/commit/ec960da1ba9ee830ba0f11abd203704fab7b885d))
* **efcore:** write enum wire names in JSON columns on EF Core 9 ([62de811](https://github.com/kathelon/CSharpEssentials/commit/62de8111b7d864c96b1f84f18ce7d8d29152d4d8)), closes [#64](https://github.com/kathelon/CSharpEssentials/issues/64)
* **endpoints:** detect duplicate routes with collection expressions ([596a7da](https://github.com/kathelon/CSharpEssentials/commit/596a7da2009914aa127f104801c1d25e47b300cb))
* **endpoints:** make generated operation names unique ([ada5fdb](https://github.com/kathelon/CSharpEssentials/commit/ada5fdb32bcd879a3eefa45fef9e2380ad887574))
* **endpoints:** skip colliding registries in the aggregate and report cse1009 ([90761eb](https://github.com/kathelon/CSharpEssentials/commit/90761eb9471938d945f13e3b9b5c83ab7f2e2819))
* **endpoints:** skip ref struct endpoints and report diagnostic ([191c9e6](https://github.com/kathelon/CSharpEssentials/commit/191c9e6e34c7204837473792730024c33143350c))
* **enums:** escape keyword member names in generated value expressions ([44d3747](https://github.com/kathelon/CSharpEssentials/commit/44d374785ea0e4397ab920503ececddf657de265)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **enums:** format a flags zero without a zero member as "0" ([b901a48](https://github.com/kathelon/CSharpEssentials/commit/b901a48268337b714f08f649c3cc1b37b7491931)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **enums:** forward IsExternalInit on .NET 5+ targets ([ca893dd](https://github.com/kathelon/CSharpEssentials/commit/ca893ddb422c4430bfc3de38b3fffc087332251d)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **enums:** qualify generated hint names with the enum namespace ([9126c33](https://github.com/kathelon/CSharpEssentials/commit/9126c33c031486d0778ff41c93a81abcc14337d4)), closes [#57](https://github.com/kathelon/CSharpEssentials/issues/57)
* **enums:** register metadata lazily and rerun module initializers on every miss ([46e0817](https://github.com/kathelon/CSharpEssentials/commit/46e0817f9e1bf9a8693a2ea968cb07617e6400e6)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **enums:** reject numeric text longer than 20 characters in the parser as the JSON converter does ([f0f2015](https://github.com/kathelon/CSharpEssentials/commit/f0f2015a3057fd6110e25aa49aa1bb8e6807ac94))
* **enums:** skip nested StringEnum generation ([#31](https://github.com/kathelon/CSharpEssentials/issues/31)) ([48bda76](https://github.com/kathelon/CSharpEssentials/commit/48bda7666d763c168a3277315ece9fc8c063d6b5))
* **enums:** throw from EnumValueFormatter when a handled enum has no generated metadata ([82967a2](https://github.com/kathelon/CSharpEssentials/commit/82967a2ad577a594f341f51cefb5fcf2f3781ffa))
* **enums:** use unique hint names and report colliding extensions classes with CSE0016 ([0c1647f](https://github.com/kathelon/CSharpEssentials/commit/0c1647ffbdebfeae321795d18f5df5bca03e62a7)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **examples:** read the sample database password from the environment instead of appsettings ([65ec645](https://github.com/kathelon/CSharpEssentials/commit/65ec64558babb0f399f1790d535f4fe9f907104b))
* **gcpsecretmanager:** reject null projects list ([4f5aa34](https://github.com/kathelon/CSharpEssentials/commit/4f5aa343c2511120d1776b435b66a59576cf01c2))
* **gcpsecretmanager:** retry transient list and access failures via resilience ([e7273aa](https://github.com/kathelon/CSharpEssentials/commit/e7273aa343501312ce400bbdb7b6560b6f53285b))
* **http:** annotate reflection based ToQueryString(object) with trimming attributes ([983c654](https://github.com/kathelon/CSharpEssentials/commit/983c654e8551c027aa1cb25937d659ff8d93c1ee))
* **http:** annotate the remaining reflection based query string overloads for trimming ([37f1503](https://github.com/kathelon/CSharpEssentials/commit/37f150378770d214056c7fc6afae2cb84d1aaf56))
* **http:** append query to relative uris without throwing ([#140](https://github.com/kathelon/CSharpEssentials/issues/140)) ([04106ee](https://github.com/kathelon/CSharpEssentials/commit/04106ee6514579a1a97047847d78d0eac52df523)), closes [#136](https://github.com/kathelon/CSharpEssentials/issues/136)
* **http:** block more reserved IPv6 and 6to4 relay ranges in SSRF guard ([904f979](https://github.com/kathelon/CSharpEssentials/commit/904f97959ace1e37d6ecda162e163b244d27d881))
* **http:** cache builder JSON options per EnumConventions instance with a weak table ([7b709e6](https://github.com/kathelon/CSharpEssentials/commit/7b709e6e7303db7ef0c503fee8445217e2a459fb))
* **http:** declare system.net.http.json as a global using for the .net 11 sdk ([9aa355d](https://github.com/kathelon/CSharpEssentials/commit/9aa355d3afb6ab937c8292bd30a501ac0e64d2f7))
* **http:** format non-enum collection items per item with invariant culture in query strings ([3c5f193](https://github.com/kathelon/CSharpEssentials/commit/3c5f193d8d88621899fe52c10f83c05e2bc6d35e))
* **http:** remove pre-6.3.0 cancellation workaround and align timeout semantics ([6e92383](https://github.com/kathelon/CSharpEssentials/commit/6e92383bc9b1e56b96b6ccf1a210b27dd1c2e97b))
* **http:** remove pre-6.3.0 cancellation workaround and align timeout semantics ([6e212c7](https://github.com/kathelon/CSharpEssentials/commit/6e212c73feba3e23e9cb75b6f4e8ac145725e033)), closes [#127](https://github.com/kathelon/CSharpEssentials/issues/127)
* **http:** restore default 30s timeout, preserve backward-compatible return types ([9db2fcc](https://github.com/kathelon/CSharpEssentials/commit/9db2fcccb4b8779bd9143bf7c79cc6b74b680963))
* **http:** serialize WithJsonContent payload at Build so later WithEnumConventions applies ([fb83fed](https://github.com/kathelon/CSharpEssentials/commit/fb83fed8ab414dd48e75d997b5a998cdbe4cbbc3))
* **json:** do not claim a StringEnum enum without metadata that CanHandle excludes ([75e1312](https://github.com/kathelon/CSharpEssentials/commit/75e1312862a7000f4b697eb51f4e8ec51c4f573c))
* **json:** keep existing JsonStringEnumConverter when adding enum conventions ([b9c945f](https://github.com/kathelon/CSharpEssentials/commit/b9c945f90879b2d90f3fa4c53ae3b72b7f10471d))
* **json:** keep type resolver and skip collection interfaces ([3225400](https://github.com/kathelon/CSharpEssentials/commit/322540056a0ebbc36d29678a66bebb3040d32717))
* **json:** reject over-long enum strings before unescaping and truncate error values ([d752591](https://github.com/kathelon/CSharpEssentials/commit/d7525910cf208d3477cf9cb7fbb3a8efb46e8aa0)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **json:** reject the JSON number -0 for enums and bound number error values ([374e199](https://github.com/kathelon/CSharpEssentials/commit/374e199e38658b23041653dbabc9921ff247e855)), closes [#61](https://github.com/kathelon/CSharpEssentials/issues/61)
* **json:** return utc from multiformatdatetimeconverter ([#141](https://github.com/kathelon/CSharpEssentials/issues/141)) ([f754e54](https://github.com/kathelon/CSharpEssentials/commit/f754e54163bd2e531a256a80993070c3cbd9beb9)), closes [#136](https://github.com/kathelon/CSharpEssentials/issues/136)
* **json:** stop writing parse failures to the console from ConvertToJsonDocument ([ce9ec4a](https://github.com/kathelon/CSharpEssentials/commit/ce9ec4aab99f0ad8bbd6d9c7974899db57e8677d))
* **json:** tolerate partially loadable assemblies when scanning for types ([7f9f780](https://github.com/kathelon/CSharpEssentials/commit/7f9f78000c2ea15a45bf2f5ee6e9d3f09f281ce5))
* **maybe:** configure the await foreach in ChooseAsync and widen the context smoke test ([8e4c149](https://github.com/kathelon/CSharpEssentials/commit/8e4c149273e24b9a8795adcb08218308f3aeaaf4))
* **maybe:** mark IMaybe async members obsolete and add overload contract tests ([fc28a61](https://github.com/kathelon/CSharpEssentials/commit/fc28a6182d5dbf01a59a7afece1468531506e813))
* **maybe:** strict equality with T, cancellation-safe FromTry, reflection-free ToString ([244d7fd](https://github.com/kathelon/CSharpEssentials/commit/244d7fd594fa2f3e3fead8e35ab4a906d3503180)), closes [#109](https://github.com/kathelon/CSharpEssentials/issues/109)
* **maybe:** use non-nullable deconstruct targets in maybe example ([4e0bfa6](https://github.com/kathelon/CSharpEssentials/commit/4e0bfa60a3e2c9110a3b6f083dc5decb093f0e05))
* **maybe:** value equality, FromTry cancellation, reflection-free ToString ([2cfe3bf](https://github.com/kathelon/CSharpEssentials/commit/2cfe3bf8e4d1e4fd0a280b424e1ebfd50b009dff))
* **mediator:** address code review findings in BehaviorCache ([eb8514c](https://github.com/kathelon/CSharpEssentials/commit/eb8514c6f2aea9090b135ec1d8586ab051a02cd7))
* **mediator:** roll back TransactionScopeBehavior on failed results ([a692870](https://github.com/kathelon/CSharpEssentials/commit/a692870751d7ebb4c1418fb954bf3e32e15650fd))
* **mediator:** use implicit conversion and await CancelAsync in tests ([4b7d4f7](https://github.com/kathelon/CSharpEssentials/commit/4b7d4f75aef8fbba6c948e595099f0177b6e3cd5))
* **openapi:** classify MVC enum parameters by their model type ([2a2323e](https://github.com/kathelon/CSharpEssentials/commit/2a2323e36670fbf68627fb8de2d62c5b6d13a2aa))
* **openapi:** describe enum properties declared on polymorphic derived types ([56adff7](https://github.com/kathelon/CSharpEssentials/commit/56adff7bf821eb5f5aa7ea64ebe28bf107a49441))
* **openapi:** drop the null branch of a required path parameter's oneOf or anyOf ([cb5c1b2](https://github.com/kathelon/CSharpEssentials/commit/cb5c1b2b1e4f5d6ebf764da13b94448d4fe7ce14))
* **openapi:** fall back to WriteAs when disposing the selector scope throws ([0378bc6](https://github.com/kathelon/CSharpEssentials/commit/0378bc65612e3ef124587ce38ce5d36e8cfde5e0))
* **openapi:** keep enum type summaries off enum properties ([efb7d53](https://github.com/kathelon/CSharpEssentials/commit/efb7d53f03e9ead45dd4b31d93be45d7d6ff3c76))
* **openapi:** match operations to API descriptions through an operation transformer ([50f2627](https://github.com/kathelon/CSharpEssentials/commit/50f2627440f5f50448e536eb18accbdf1f6f8120))
* **openapi:** run header format selectors in a scope and fall back to WriteAs when they throw ([ec21509](https://github.com/kathelon/CSharpEssentials/commit/ec215091382b1d9d67eac11c49506485058d7a35))
* **openapi:** validate the optional route parameter mode with a pattern ([9d0f855](https://github.com/kathelon/CSharpEssentials/commit/9d0f8559e95512f4e90b049b866ad354ba4f83d4))
* **openapi:** write ulong enum values above long.MaxValue exactly in both packages ([ab3c495](https://github.com/kathelon/CSharpEssentials/commit/ab3c495ff2c24f2e7df2823a3a882a8ea6cabda5))
* **resilience:** address review — RetryIfFailed filter, pragma, docs, SKILL.md ([ba0546b](https://github.com/kathelon/CSharpEssentials/commit/ba0546b250f58e3362485d0b6d0c268a3cf70662))
* **resilience:** classify cancellation by the caller token ([d900a89](https://github.com/kathelon/CSharpEssentials/commit/d900a895f906a1f44c0a9115d55dc894464ec81b))
* **resilience:** FallbackAction exception handling, RetryIfFailed exception support, fix docs ([e69228b](https://github.com/kathelon/CSharpEssentials/commit/e69228b257d4b544d236f42d6fe1450f51849845))
* **resilience:** fix remaining broken examples in Readme.MD ([f37b2c8](https://github.com/kathelon/CSharpEssentials/commit/f37b2c89c81ccd192aef3b72f7ba4e238a060901))
* **resilience:** fix RetryIfFailed example and Merge null guard ([77931dd](https://github.com/kathelon/CSharpEssentials/commit/77931ddb5012f994ff4408cf9c1486fe52442efc))
* **resilience:** guard default policy pipeline and propagate cancellation ([aa2536b](https://github.com/kathelon/CSharpEssentials/commit/aa2536b38a53a07954252fa2661d6a136e2c15e7))
* **resilience:** null guard for default struct, fix Quick Start examples ([440545f](https://github.com/kathelon/CSharpEssentials/commit/440545fa7e1c2cd30758f2ea5f3bc38dcd2e4e0e))
* **resilience:** throw on caller cancellation after a failed policy attempt ([bff0652](https://github.com/kathelon/CSharpEssentials/commit/bff065280d22f6dbd53a8133f9cee0c50cf93941))
* **results:** await async handlers in conditional TapAsync on Task and ValueTask sources ([df94a7e](https://github.com/kathelon/CSharpEssentials/commit/df94a7e253e22638a46d28959822ec227e001873))
* **results:** await async handlers in conditional TapAsync on Task and ValueTask sources ([4724c1f](https://github.com/kathelon/CSharpEssentials/commit/4724c1f918ace9256ff9dc8f9d4a3946407927d0))
* **results:** cancel and observe pending tasks in SequenceAsync ([cf5b8da](https://github.com/kathelon/CSharpEssentials/commit/cf5b8da55cb20c74c5b9c0b23770731158ecc7ff)), closes [#108](https://github.com/kathelon/CSharpEssentials/issues/108)
* **results:** clarify async naming docs and deprecate interface Bind overloads ([ffbcae4](https://github.com/kathelon/CSharpEssentials/commit/ffbcae4c65a457b5ac8917aef862ff9c87e1eacd)), closes [#108](https://github.com/kathelon/CSharpEssentials/issues/108)
* **results:** map every error in MapError and add MapErrorAsync ([dcd8b89](https://github.com/kathelon/CSharpEssentials/commit/dcd8b896d5b1d8bead362d72f48c3c54d6098b8e))
* **results:** map every error in MapError(Func&lt;Error, Error&gt;) ([8778617](https://github.com/kathelon/CSharpEssentials/commit/87786173348f203d2ef1f1918dc2cd7378bd894f)), closes [#107](https://github.com/kathelon/CSharpEssentials/issues/107)
* **results:** propagate caller cancellation from TryAsync ([687945d](https://github.com/kathelon/CSharpEssentials/commit/687945dda89aab47f72dc3a67ec8dfe1b4836839))
* **results:** propagate caller cancellation from TryAsync instead of converting it to an Error ([a126b51](https://github.com/kathelon/CSharpEssentials/commit/a126b51d8c6ec13ba5e7f4d78d958ed06150091f))
* **rules:** document that sync Evaluate throws caller cancellation for async root rules ([1800cce](https://github.com/kathelon/CSharpEssentials/commit/1800cce4b687048c4abf4e1b3296e40881d0a212))
* **rules:** keep every rule when chaining next calls ([#138](https://github.com/kathelon/CSharpEssentials/issues/138)) ([7399565](https://github.com/kathelon/CSharpEssentials/commit/7399565e76f53ed52f740a2fe9f9b0569b23be15)), closes [#136](https://github.com/kathelon/CSharpEssentials/issues/136)
* SSRF range and pagination overflow hardening ([c90df02](https://github.com/kathelon/CSharpEssentials/commit/c90df02fc114e4e7107071d15e1b755016cd296d))
* **tests:** resolve all analyzer warnings in test projects ([4a681a6](https://github.com/kathelon/CSharpEssentials/commit/4a681a61cc06028b12199c3003f3564c1dde78c5))
* **tests:** resolve all analyzer warnings in test projects ([ff152eb](https://github.com/kathelon/CSharpEssentials/commit/ff152ebcdf973b7bc1a504ef366c9b379848ccec))
* **tests:** resolve analyzer warnings in resilience and enum tests ([cea3f30](https://github.com/kathelon/CSharpEssentials/commit/cea3f3041cf32302c38f08ed0375d04d09a151a7))
* **these:** keep the error of both in flatmap ([#143](https://github.com/kathelon/CSharpEssentials/issues/143)) ([79f2aac](https://github.com/kathelon/CSharpEssentials/commit/79f2aac6fd8eb2a5bfdb09ba955ca44a48723088)), closes [#136](https://github.com/kathelon/CSharpEssentials/issues/136)
* **time:** return utc kind from datetimeprovider.utcnowdatetime ([#147](https://github.com/kathelon/CSharpEssentials/issues/147)) ([83f7ff3](https://github.com/kathelon/CSharpEssentials/commit/83f7ff393c71390543636d70ff22a20abbc5f36e)), closes [#146](https://github.com/kathelon/CSharpEssentials/issues/146)
* **validation:** normalise property paths for filtered collections and null operators ([#144](https://github.com/kathelon/CSharpEssentials/issues/144)) ([e2760e0](https://github.com/kathelon/CSharpEssentials/commit/e2760e090921e43dbc798d4246c0f048a4ee645b)), closes [#136](https://github.com/kathelon/CSharpEssentials/issues/136)
* **validation:** prefix the quoted property name in nested error descriptions ([eea76b0](https://github.com/kathelon/CSharpEssentials/commit/eea76b095c61ffaecf83cee66787303138acbd65))
* **validation:** skip duplicate validator registrations ([f2cc998](https://github.com/kathelon/CSharpEssentials/commit/f2cc9986c43e28cad353fce1647a10e2ec23f6dc))
* **validation:** support nullable types and concrete collections in validators ([df17a0d](https://github.com/kathelon/CSharpEssentials/commit/df17a0dbd6a998c91489d5a936f3270acce6512c))
* **validation:** tolerate null intermediate segments in member paths ([ef4e6a1](https://github.com/kathelon/CSharpEssentials/commit/ef4e6a147442bda7fdc8cfdabb8870b576b789eb))
* **validation:** use the {Prop}.{Rule} format for enum validator error codes ([cf84856](https://github.com/kathelon/CSharpEssentials/commit/cf84856d6c9802fa04e1fcfdcec0e662078dbf33))


### Changed

* **aspnetcore:** copy request query and form only when an enum binding value changes ([a02bde2](https://github.com/kathelon/CSharpEssentials/commit/a02bde2d946d094412a7a53f7dad7ae6d620515d))
* **core:** skip allocations in WithCancellation when task completed or token cannot cancel ([f94681c](https://github.com/kathelon/CSharpEssentials/commit/f94681c41421e98d2789aa207c0c2cd3cfe13872))
* **core:** skip allocations in WithCancellation when task completed or token cannot cancel ([f9553f9](https://github.com/kathelon/CSharpEssentials/commit/f9553f9a299c6ed9c09b15a92da671a698aaea65))
* **efcore:** drop the per-write closure in EnumColumnCodec ([2a9139e](https://github.com/kathelon/CSharpEssentials/commit/2a9139e41b3fefaabb2830907830fb8eeb4a0aa1))
* **efcore:** obsolete single-column cursor PaginateAsync ([d431ad7](https://github.com/kathelon/CSharpEssentials/commit/d431ad7a4d02134219450cb3c02c1868b8bc6fe4))
* **endpoints:** filter endpoint name invocations by method name first ([c7396c4](https://github.com/kathelon/CSharpEssentials/commit/c7396c434bf33125b6942d9c76ff8b3a21a5190e))
* **gcpsecretmanager:** make options a record and log via ILogger ([86c5dc9](https://github.com/kathelon/CSharpEssentials/commit/86c5dc97a419b6bdf6d3eee50060b7d6618fa3d2)), closes [#8](https://github.com/kathelon/CSharpEssentials/issues/8)
* **json:** drop the per-call delegate array in ConvertToJsonDocument ([1329d19](https://github.com/kathelon/CSharpEssentials/commit/1329d19baccd03a7f4aff15afa4fb69e65cef4ad))
* **json:** parse a number token once in the enum converter ([2130423](https://github.com/kathelon/CSharpEssentials/commit/21304237bfdc30441cf15ef36daef727ccce1cdb))
* **json:** read an over-long unknown enum value as the fallback without allocating its string ([87b06a7](https://github.com/kathelon/CSharpEssentials/commit/87b06a736c59e373fbe1501cb0d8aa70ce9e3113))
* **resilience:** reuse retry pipelines in RetryIfFailed ([d48e74d](https://github.com/kathelon/CSharpEssentials/commit/d48e74de97ca7c8d343f8baaadcaa7ac43344582))
* **results:** add async matrix benchmarks ([7a7a44b](https://github.com/kathelon/CSharpEssentials/commit/7a7a44b3d1c9e95a959c1904e5fafbf6f1c3a840))
* **results:** add Else and TapError async matrix benchmarks ([ba2d839](https://github.com/kathelon/CSharpEssentials/commit/ba2d8394857f87b1cc907f38d51e03ba3e34e693))
* **results:** check token instead of WithCancellation in MapErrorAsync mappers ([4c35599](https://github.com/kathelon/CSharpEssentials/commit/4c3559977d209b072ed655df11b75a9f97e6184d))
* **results:** zero-allocation failure ROP pipelines (resolved conflicts) ([#22](https://github.com/kathelon/CSharpEssentials/issues/22)) ([30f6be1](https://github.com/kathelon/CSharpEssentials/commit/30f6be1dbda9caa08865366d081eb57afa4bb931))
* **validation:** reduce allocations on valid path via lazy error list and Span parsing ([ff59afa](https://github.com/kathelon/CSharpEssentials/commit/ff59afa783d121d494d296a1438c6faa88baa416))


### Build

* **deps:** depend on polly.core instead of polly ([515de1f](https://github.com/kathelon/CSharpEssentials/commit/515de1f2faada04d166b02861d00159a319253e2))
* **efcore:** compile each target framework against its own ef core major ([8109203](https://github.com/kathelon/CSharpEssentials/commit/810920326dc9c86ad58e5662ef5433469813bc7f)), closes [#7](https://github.com/kathelon/CSharpEssentials/issues/7)
* **enums:** let every csharpessentials package bring the enums generator and analyzers ([5998b81](https://github.com/kathelon/CSharpEssentials/commit/5998b81bef6a7360892bf8496a5a4fba87b04a7e))
* **enums:** let the enums generator and analyzers flow through errors, results, openapi and swashbuckle ([383e6ab](https://github.com/kathelon/CSharpEssentials/commit/383e6ab872e98055b02161006bc7222e842fce28))
* **enums:** let the enums generator and analyzers flow through json, efcore, aspnetcore, http and the meta-package ([2fe6ba7](https://github.com/kathelon/CSharpEssentials/commit/2fe6ba7759eb45624c60084812f451119ad451bf))

## [6.5.2](https://github.com/senrecep/CSharpEssentials/compare/v6.5.1...v6.5.2) (2026-10-10)


### Fixed

* **time:** return utc kind from datetimeprovider.utcnowdatetime ([#147](https://github.com/senrecep/CSharpEssentials/issues/147)) ([83f7ff3](https://github.com/senrecep/CSharpEssentials/commit/83f7ff393c71390543636d70ff22a20abbc5f36e)), closes [#146](https://github.com/senrecep/CSharpEssentials/issues/146)

## [6.5.1](https://github.com/senrecep/CSharpEssentials/compare/v6.5.0...v6.5.1) (2026-10-09)


### Fixed

* **core:** bound stackalloc and validate guider input ([#142](https://github.com/senrecep/CSharpEssentials/issues/142)) ([2eddfba](https://github.com/senrecep/CSharpEssentials/commit/2eddfba5a4220efc54aa46d8b734517bfe686cc8)), closes [#136](https://github.com/senrecep/CSharpEssentials/issues/136)
* **http:** append query to relative uris without throwing ([#140](https://github.com/senrecep/CSharpEssentials/issues/140)) ([04106ee](https://github.com/senrecep/CSharpEssentials/commit/04106ee6514579a1a97047847d78d0eac52df523)), closes [#136](https://github.com/senrecep/CSharpEssentials/issues/136)
* **json:** return utc from multiformatdatetimeconverter ([#141](https://github.com/senrecep/CSharpEssentials/issues/141)) ([f754e54](https://github.com/senrecep/CSharpEssentials/commit/f754e54163bd2e531a256a80993070c3cbd9beb9)), closes [#136](https://github.com/senrecep/CSharpEssentials/issues/136)
* **rules:** keep every rule when chaining next calls ([#138](https://github.com/senrecep/CSharpEssentials/issues/138)) ([7399565](https://github.com/senrecep/CSharpEssentials/commit/7399565e76f53ed52f740a2fe9f9b0569b23be15)), closes [#136](https://github.com/senrecep/CSharpEssentials/issues/136)
* **these:** keep the error of both in flatmap ([#143](https://github.com/senrecep/CSharpEssentials/issues/143)) ([79f2aac](https://github.com/senrecep/CSharpEssentials/commit/79f2aac6fd8eb2a5bfdb09ba955ca44a48723088)), closes [#136](https://github.com/senrecep/CSharpEssentials/issues/136)
* **validation:** normalise property paths for filtered collections and null operators ([#144](https://github.com/senrecep/CSharpEssentials/issues/144)) ([e2760e0](https://github.com/senrecep/CSharpEssentials/commit/e2760e090921e43dbc798d4246c0f048a4ee645b)), closes [#136](https://github.com/senrecep/CSharpEssentials/issues/136)

## [6.5.0](https://github.com/senrecep/CSharpEssentials/compare/v6.4.0...v6.5.0) (2026-10-09)


### Added

* **maybe:** `Async`-suffixed names for the task-returning members left from 6.4.0: `ExecuteAsync`, `ExecuteNoValueAsync`, `OrAsync`, `MatchAsync`, `ToMaybeResultAsync`, `ToMaybeUnitResultAsync` and `Maybe.FromAsync` ([2ec0851](https://github.com/senrecep/CSharpEssentials/commit/2ec0851325a24f46694a6dc56440c9e406997a0b)), closes [#121](https://github.com/senrecep/CSharpEssentials/issues/121)
* **results:** `ValueTask` twins on .NET 9+ for `ElseAsync`, `FailIfAsync`, `SwitchFirstAsync`, `SwitchLastAsync`, `TapErrorAsync`, `TapErrorFirstAsync`, `ElseDoAsync`, `ElseDoFirstAsync`, `CompensateAsync`, `CompensateFirstAsync`, `ThenEnsureAsync`, `FinallyAsync` (on `ValueTask` sources) and `Result.TryAsync` ([5f1a0db](https://github.com/senrecep/CSharpEssentials/commit/5f1a0dbcfaec2a2ab17a34b5e3c94688df9ea6cb)), closes [#122](https://github.com/senrecep/CSharpEssentials/issues/122)


### Fixed

* **results:** the conditional `TapAsync(bool | Func<bool>, …)` on `Task` and `ValueTask` sources has awaited `Func<Task>`/`Func<ValueTask>` handlers, so an `async` lambda no longer runs as `async void` ([4724c1f](https://github.com/senrecep/CSharpEssentials/commit/4724c1f918ace9256ff9dc8f9d4a3946407927d0)), closes [#124](https://github.com/senrecep/CSharpEssentials/issues/124)
* **http:** remove the pre-6.3.0 cancellation workaround; only the caller's cancellation throws, an `HttpClient.Timeout` or any other cancellation becomes a failed `Result` ([6e212c7](https://github.com/senrecep/CSharpEssentials/commit/6e212c73feba3e23e9cb75b6f4e8ac145725e033)), closes [#127](https://github.com/senrecep/CSharpEssentials/issues/127)
* add `ConfigureAwait(false)` to about 800 awaits across the library packages and enforce CA2007 for them ([c54f124](https://github.com/senrecep/CSharpEssentials/commit/c54f1240c0a1bcb653f2ff23f17e3c0240126c7a), [8e4c149](https://github.com/senrecep/CSharpEssentials/commit/8e4c149273e24b9a8795adcb08218308f3aeaaf4)), closes [#123](https://github.com/senrecep/CSharpEssentials/issues/123)


### Changed

* **results:** add Else and TapError async matrix benchmarks ([ba2d839](https://github.com/senrecep/CSharpEssentials/commit/ba2d8394857f87b1cc907f38d51e03ba3e34e693))
* **maybe:** document the null equality rule: a typed `null` means absence, `Equals(object?)` follows the BCL contract ([8a387ef](https://github.com/senrecep/CSharpEssentials/commit/8a387ef)), closes [#125](https://github.com/senrecep/CSharpEssentials/issues/125)
* **results:** document that `Result.And` short-circuits on the first failure and that `CombineAll` collects every error ([beca7d4](https://github.com/senrecep/CSharpEssentials/commit/beca7d4)), closes [#126](https://github.com/senrecep/CSharpEssentials/issues/126)


### Behaviour changes

* **http:** `HttpClient.Timeout` and any cancellation that is not the caller's token now return a failed `Result` instead of throwing `OperationCanceledException`. A timeout gives `Http.Timeout` (`ErrorType.Unexpected`), any other cancellation uses the exception type name as the code. Cancelling the caller's own token still throws. Code that wraps these calls in `catch (OperationCanceledException)` to handle timeouts must check the `Result` instead. Timeouts raised by Polly resilience handlers keep `TimeoutRejectedException` as the code.
* **results:** an `async` lambda passed to the conditional `TapAsync` on a `Task` or `ValueTask` source is now awaited and its exception reaches the caller; it used to run as `async void`. A non-`async` lambda that returns the source's own flavour (`Task` on a `Task` source, `ValueTask` on a `ValueTask` source) is awaited too.
* **results:** on .NET 9+, `SwitchFirstAsync` and `SwitchLastAsync` on `ValueTask` sources await lambdas that return a `ValueTask`; they used to bind the `Action` pair and discard them. `FinallyAsync` on `ValueTask` sources now awaits a lambda that returns `ValueTask`, `ValueTask<T>` or `Task<T>` and returns `ValueTask<Result…>` or `ValueTask<T>`, where it used to return a nested task.
* the library no longer resumes continuations on the caller's `SynchronizationContext`, so sync-over-async callers on a single-threaded context no longer deadlock inside library code.


### Deprecations

* **maybe:** the unsuffixed task-returning `Execute`, `ExecuteNoValue`, `Or`, `Match`, `ToMaybeResult`, `ToMaybeUnitResult` and `Maybe.From(Task…)` overloads, and the matching async members of `IMaybe<T>`, are `[Obsolete]` forwarders to the `*Async` names and are removed in 7.0. Projects with `TreatWarningsAsErrors` will see CS0618 — see `docs/migration/v6-async-naming.md`.


### Source compatibility

* .NET 9+ consumers that pin `LangVersion` 12 get CS0121 for untyped async lambdas on the new `ValueTask` twins. The `Maybe` instance and key/value `ExecuteAsync`, `ExecuteNoValueAsync`, `OrAsync` and `MatchAsync` were already ambiguous on every language version; they now bind the `Task` overload on C# 13+ and still need a typed handler on C# 12, on .NET 8 as well. No binary breaks.

## [6.4.0](https://github.com/senrecep/CSharpEssentials/compare/v6.3.0...v6.4.0) (2026-10-08)


### Added

* **results:** add Async-suffixed names for task-returning members ([18e8b75](https://github.com/senrecep/CSharpEssentials/commit/18e8b75fda126cf3b8cc88b8b93fa26ff352abf0)), closes [#108](https://github.com/senrecep/CSharpEssentials/issues/108)
* **results:** add sequential TraverseAsync and SequenceAsync ([dc9fbcc](https://github.com/senrecep/CSharpEssentials/commit/dc9fbcc22666e08354927ae857fc556c95685bd2)), closes [#108](https://github.com/senrecep/CSharpEssentials/issues/108)
* **results:** fill flavour-matched async matrix ([e8f4f49](https://github.com/senrecep/CSharpEssentials/commit/e8f4f49ab413ba9cdf6a5f06e5fe455b6a6d29a3)), closes [#108](https://github.com/senrecep/CSharpEssentials/issues/108)


### Fixed

* **maybe:** strict equality with T, cancellation-safe FromTry, reflection-free ToString ([244d7fd](https://github.com/senrecep/CSharpEssentials/commit/244d7fd594fa2f3e3fead8e35ab4a906d3503180)), closes [#109](https://github.com/senrecep/CSharpEssentials/issues/109)
* **results:** cancel and observe pending tasks in SequenceAsync ([cf5b8da](https://github.com/senrecep/CSharpEssentials/commit/cf5b8da55cb20c74c5b9c0b23770731158ecc7ff)), closes [#108](https://github.com/senrecep/CSharpEssentials/issues/108)
* **results:** clarify async naming docs and deprecate interface Bind overloads ([ffbcae4](https://github.com/senrecep/CSharpEssentials/commit/ffbcae4c65a457b5ac8917aef862ff9c87e1eacd)), closes [#108](https://github.com/senrecep/CSharpEssentials/issues/108)


### Changed

* **results:** add async matrix benchmarks ([7a7a44b](https://github.com/senrecep/CSharpEssentials/commit/7a7a44b3d1c9e95a959c1904e5fafbf6f1c3a840))


### Behaviour changes

* **maybe:** equality with a raw value is strict — a non-null value never equals `None` (`Maybe<int>.None == 0` is now `false`); `None == null` is `true` in either operand order, `Some(x) == null` is `false`.
* **maybe:** `FromTry` no longer swallows `OperationCanceledException`; cancellation propagates to the caller.
* **maybe:** `ToString()` returns `Some(value)` / `None` (invariant culture, no reflection/JSON).
* **results:** `TapAsync` with an async lambda on a `Task`/`ValueTask` source result is now awaited (previously ran as `async void`, losing exceptions); a non-async Task-returning lambda on a `Task` source is now awaited too.
* **results:** `ValueTask<Result<T>>.MapAsync(async …)` now returns `ValueTask<Result<U>>` instead of `ValueTask<Result<Task<U>>>`.
* **results:** on .NET 9+, a `ValueTask`-returning lambda binds to the new `ValueTask` handler instead of the sync/`Action` overload.


### Deprecations

* **results:** unsuffixed task-returning members (`Bind`, `Else`, `FailIf`, `Match*`, `Switch*`, `Then`, `ThenDo`, Maybe `Where`) are `[Obsolete]` forwarders to their `*Async` names, and the Task/ValueTask `Bind` members of `IResult`/`IResult<T>` are obsolete; all are removed in 7.0. Projects with `TreatWarningsAsErrors` will see CS0618 — see `docs/migration/v6-async-naming.md`.
* **maybe:** `IsNone` is obsolete; use `HasNoValue`.


### Source compatibility

* .NET 9+ consumers that pin `LangVersion` 12 get CS0121 for untyped async lambdas on `Match*`, `Switch`, `Ensure`, `TapIf`, `Then`, `ThenDo` and `TraverseAsync` (C# 12 ignores `OverloadResolutionPriority`). Use C# 13+ (the .NET 9 default) or a typed handler. No binary breaks.

## [6.3.0](https://github.com/senrecep/CSharpEssentials/compare/v6.2.0...v6.3.0) (2026-10-08)


### Behaviour changes

* **results:** `MapError(Func<Error, Error>)` now maps every error of a failed result, in order. It used to map only the first error and drop the rest.
* **results:** `TryAsync` no longer turns the caller's cancellation into an error `Result`. When the passed token is cancelled, `OperationCanceledException` propagates. Cancellation from any other token still becomes an error. This also applies to `SaveChangesAsResultAsync` and to async rules evaluated by `RuleEngine.Evaluate` unless an enclosing sync rule returns it as an error `Result`.


### Added

* **efcore:** add nullable Maybe&lt;T&gt;? column mapping ([a42be18](https://github.com/senrecep/CSharpEssentials/commit/a42be18177869a51bdf56101fe10e5489aba427c)), closes [#106](https://github.com/senrecep/CSharpEssentials/issues/106)
* **maybe:** add Flatten for Maybe&lt;T&gt;? ([8765bc9](https://github.com/senrecep/CSharpEssentials/commit/8765bc95d035dbf122b5bbf4fd020e3111b9bbde))
* **results:** add MapErrorAsync overloads ([e0e78e2](https://github.com/senrecep/CSharpEssentials/commit/e0e78e2982b2f2425ca30636f0053b0539e20d72))
* **rules:** add async RuleEngine.EvaluateAsync that awaits async rules ([e66f8bc](https://github.com/senrecep/CSharpEssentials/commit/e66f8bcb9c76c594d4bca7d5c8243f00d57279e4))


### Fixed

* **efcore:** map Maybe&lt;T&gt;? in complex types and document convention limits ([10ca7ef](https://github.com/senrecep/CSharpEssentials/commit/10ca7efe1de4fcbbe2a7236000e430bdd1886531))
* **results:** map every error in MapError(Func&lt;Error, Error&gt;) ([8778617](https://github.com/senrecep/CSharpEssentials/commit/87786173348f203d2ef1f1918dc2cd7378bd894f)), closes [#107](https://github.com/senrecep/CSharpEssentials/issues/107)
* **results:** propagate caller cancellation from TryAsync instead of converting it to an Error ([a126b51](https://github.com/senrecep/CSharpEssentials/commit/a126b51d8c6ec13ba5e7f4d78d958ed06150091f))
* **rules:** document that sync Evaluate throws caller cancellation for async root rules ([1800cce](https://github.com/senrecep/CSharpEssentials/commit/1800cce4b687048c4abf4e1b3296e40881d0a212))


### Changed

* **core:** skip allocations in WithCancellation when task completed or token cannot cancel ([f9553f9](https://github.com/senrecep/CSharpEssentials/commit/f9553f9a299c6ed9c09b15a92da671a698aaea65))

## [6.2.0](https://github.com/senrecep/CSharpEssentials/compare/v6.1.0...v6.2.0) (2026-10-07)


### Added

* **openapi:** describe optional route parameters in Microsoft.AspNetCore.OpenApi documents ([33855a3](https://github.com/senrecep/CSharpEssentials/commit/33855a3860a6efa0812cb3865752b04d0f2c0b24))
* **openapi:** warn when SplitPaths keeps an operation on one path ([593de9b](https://github.com/senrecep/CSharpEssentials/commit/593de9b53fdefc62c337a2cad26b613d1ac07ce7))


### Fixed

* **openapi:** drop the `{type: null}` branch of a required path parameter's oneOf or anyOf (nullable enum route parameters on OpenAPI 3.1), also in the Swashbuckle package ([cb5c1b2](https://github.com/senrecep/CSharpEssentials/commit/cb5c1b2b1e4f5d6ebf764da13b94448d4fe7ce14))

## [6.1.0](https://github.com/senrecep/CSharpEssentials/compare/v6.0.0...v6.1.0) (2026-10-07)


### Added

* **openapi:** emit spec-valid optional route parameters by splitting paths ([e32afea](https://github.com/senrecep/CSharpEssentials/commit/e32afea0328900e37e7355cac9a21c9cc6f09a77)), closes [#101](https://github.com/senrecep/CSharpEssentials/issues/101)

## [6.0.0](https://github.com/senrecep/CSharpEssentials/compare/v5.2.1...v6.0.0) (2026-10-07)


### ⚠ BREAKING CHANGES

* **openapi:** CSharpEssentials.AspNetCore.Swashbuckle needs Swashbuckle.AspNetCore [10.2.3,11) and Microsoft.OpenApi [2.7.5,3) on every target. AddSwagger<T>(securityScheme, assembly) is now AddSwagger<T>(securitySchemeName, securityScheme, assembly); pass SecuritySchemes.JwtBearerSchemeName ("Bearer") for the previous behavior. SecuritySchemes.JwtBearerTokenSecurity no longer sets Reference. Custom Swashbuckle filters and MapType factories must move to the Microsoft.OpenApi 2.x API (using Microsoft.OpenApi; IOpenApiSchema; JsonSchemaType; JsonNode).
* **efcore:** offset pagination returns at most 100 rows per page unless a larger cap is given. Pass maxPageSize: 500 (or int.MaxValue for the 5.x behavior) to PaginateAsync/Paginate, or call Normalize(int.MaxValue). The sync Paginate overloads gained a parameter, so assemblies compiled against 5.x must be recompiled. Custom IPaginationRequest types that override Normalize() must also override Normalize(int). See docs/migration/v5-to-v6.md.
* **efcore:** the single-column cursor `PaginateAsync<T, TCursor>` is `[Obsolete]`; use `KeysetPaginateAsync`, which supports composite keys, opaque cursors, backward paging and a maximum limit ([d431ad7](https://github.com/senrecep/CSharpEssentials/commit/d431ad7)).

### Added

* **efcore:** cap offset page size at 100 by default ([74badad](https://github.com/senrecep/CSharpEssentials/commit/74badad9db80607a798a41c282e064a2d3578985))
* **openapi:** require Swashbuckle 10 and Microsoft.OpenApi 2 on all targets ([ca352cf](https://github.com/senrecep/CSharpEssentials/commit/ca352cf0675b2105adbbae6adb0fc4dc2fdd1d91)), closes [#69](https://github.com/senrecep/CSharpEssentials/issues/69)


### Fixed

* **openapi:** keep enum type summaries off enum properties ([efb7d53](https://github.com/senrecep/CSharpEssentials/commit/efb7d53f03e9ead45dd4b31d93be45d7d6ff3c76))

## [5.2.1](https://github.com/senrecep/CSharpEssentials/compare/v5.2.0...v5.2.1) (2026-10-07)


### Fixed

* **efcore:** prevent int overflow in pagination skip and take ([07bfc4a](https://github.com/senrecep/CSharpEssentials/commit/07bfc4a879023499bfe7be343534b9c38d39ee26))
* **http:** block more reserved IPv6 and 6to4 relay ranges in SSRF guard ([904f979](https://github.com/senrecep/CSharpEssentials/commit/904f97959ace1e37d6ecda162e163b244d27d881))

## [5.2.0](https://github.com/senrecep/CSharpEssentials/compare/v5.1.0...v5.2.0) (2026-10-07)


### Added

* **efcore:** add opt-in max limit to pagination request normalization ([4a77d5b](https://github.com/senrecep/CSharpEssentials/commit/4a77d5b445342039cf4711d8e923cc512cc0efcc)) (refs [#92](https://github.com/senrecep/CSharpEssentials/issues/92))
* **http:** add SSRF guard for outbound HttpClient requests ([c97b0cc](https://github.com/senrecep/CSharpEssentials/commit/c97b0cc1a7df6c48d43ec188053ecdf7afe8cdf6)), closes [#80](https://github.com/senrecep/CSharpEssentials/issues/80)

## [5.1.0](https://github.com/senrecep/CSharpEssentials/compare/v5.0.0...v5.1.0) (2026-10-07)


### Added

* **aspnetcore:** add conditional GET (ETag/Last-Modified, 304) and If-Match preconditions ([5f1e229](https://github.com/senrecep/CSharpEssentials/commit/5f1e22935277b00d7d53849f80738a536679fe4b)), closes [#78](https://github.com/senrecep/CSharpEssentials/issues/78)
* **aspnetcore:** add Idempotency-Key middleware with pluggable IIdempotencyStore ([60caf33](https://github.com/senrecep/CSharpEssentials/commit/60caf33c7ee68f60e7082ffd447d85d8e3224832)), closes [#79](https://github.com/senrecep/CSharpEssentials/issues/79)
* **efcore:** add composite keyset pagination with opaque cursors ([3835744](https://github.com/senrecep/CSharpEssentials/commit/38357441eb23b42146b9fe2a1eea46dafab38e9c)), closes [#87](https://github.com/senrecep/CSharpEssentials/issues/87)
* **efcore:** add EF Core 10 named query filter helpers and IgnoreQueryFilters analyzer ([3e91857](https://github.com/senrecep/CSharpEssentials/commit/3e91857d981ee0be19db80b277769f263fdac6d0)), closes [#77](https://github.com/senrecep/CSharpEssentials/issues/77)
* **efcore:** add IDbErrorTranslator with SQLSTATE default ([ad06306](https://github.com/senrecep/CSharpEssentials/commit/ad0630634c0df084668f476470b43670f21e8003)), closes [#74](https://github.com/senrecep/CSharpEssentials/issues/74)
* **mediator:** add IResourceLock and LockBehavior ([2d08bfb](https://github.com/senrecep/CSharpEssentials/commit/2d08bfb17b00873314acc48a60a89d924c71c9f3)), closes [#76](https://github.com/senrecep/CSharpEssentials/issues/76)
* **mediator:** add ITransactionRunner and TransactionBehavior ([105a3ac](https://github.com/senrecep/CSharpEssentials/commit/105a3ac6f74fb8c53ebdbd43d7a27d3c1f227245)), closes [#72](https://github.com/senrecep/CSharpEssentials/issues/72)
* **mediator:** add validation modes and failure observers ([ecf9873](https://github.com/senrecep/CSharpEssentials/commit/ecf98739cfe431413a26ccaec1b22dba6fcd7eed)), closes [#73](https://github.com/senrecep/CSharpEssentials/issues/73)


### Fixed

* **mediator:** roll back TransactionScopeBehavior on failed results ([a692870](https://github.com/senrecep/CSharpEssentials/commit/a692870751d7ebb4c1418fb954bf3e32e15650fd))

## [5.0.0](https://github.com/senrecep/CSharpEssentials/compare/v4.1.0...v5.0.0) (2026-10-06)


### ⚠ BREAKING CHANGES

* **enums:** the CSE enum analyzers (CSE0002-CSE0016) and the generator now run in every project that references any CSharpEssentials package that depends on CSharpEssentials.Enums, including .Validation, .Mediator, .Any, .Maybe, .Entity, .Rules, .These, .Resilience and .GcpSecretManager. New CSE warnings and errors can appear; fix them or set their severity in .editorconfig.
* **enums:** the CSE enum analyzers (CSE0002-CSE0016) and the generator now also run in every project that references CSharpEssentials.Errors, .Results, .AspNetCore.OpenApi or .AspNetCore.Swashbuckle. New CSE diagnostics can appear; fix them or set their severity in .editorconfig.
* **enums:** the CSE enum analyzers (CSE0002-CSE0016) and the generator now run in every project that references CSharpEssentials.Json, .EntityFrameworkCore, .AspNetCore, .Http or the CSharpEssentials meta-package. New CSE warnings can appear (errors under TreatWarningsAsErrors); fix them or set their severity in .editorconfig.
* **swashbuckle:** EnumSchemaFilter takes an IServiceProvider and no longer writes the "Possible values" description.
* **aspnetcore:** hosts that use AddSwagger or the Swagger filters must reference CSharpEssentials.AspNetCore.Swashbuckle.
* **json:** enums without generated metadata are no longer converted by AddEnumConventions, ConditionalStringEnumConverter or a custom CanHandle predicate; use AddEnumConventionsWithReflection. StringEnumNaming, and the EF Core and binding helpers built on it, throw InvalidOperationException for such enums.
* **json:** ConditionalStringEnumConverter.AllowUndefinedValues is removed and undefined values are rejected on read and write. Naming policies other than SnakeCaseLower throw NotSupportedException; set the naming at build time instead.

### Added

* **aspnetcore:** add AddEnumConventions, enum wire format and route binding through the conventions ([331d94b](https://github.com/senrecep/CSharpEssentials/commit/331d94bca1201be90def9807bcc17d1c64da2719))
* **aspnetcore:** move the Swagger support to CSharpEssentials.AspNetCore.Swashbuckle ([e74f257](https://github.com/senrecep/CSharpEssentials/commit/e74f25754d1647f1ddd33c343cdfc8ecc28b1a6e))
* **efcore:** add enum column migration helpers and data audit ([ad5d7a4](https://github.com/senrecep/CSharpEssentials/commit/ad5d7a48caab62cd13c309e5703e2fbfb56e4fb3))
* **efcore:** store enums as wire names or integers with check constraints ([59c81f9](https://github.com/senrecep/CSharpEssentials/commit/59c81f93c769524e71606071abfccf202b6a6a5d)), closes [#64](https://github.com/senrecep/CSharpEssentials/issues/64)
* **enums:** add analyzers for wire names, aliases, fallbacks and flags ([0f1d441](https://github.com/senrecep/CSharpEssentials/commit/0f1d441593f115debd6ee9c9a8271a1ac208e192))
* **enums:** add code fixes for explicit values and none member ([57f8a8a](https://github.com/senrecep/CSharpEssentials/commit/57f8a8a4ab9189b2ceceecaea3777ba47f0888e7))
* **enums:** add enum metadata, conventions, parser and formatter ([c2d3d66](https://github.com/senrecep/CSharpEssentials/commit/c2d3d66a5fe161a422a0356110b47b808899f2d8)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **enums:** add non-generic EnumValueFormatter overloads ([bfb237b](https://github.com/senrecep/CSharpEssentials/commit/bfb237bb0f6d771e041c1a934558868c88d3ae32)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **enums:** generate enum metadata, registration and wire-name helpers ([80c0689](https://github.com/senrecep/CSharpEssentials/commit/80c06897b6ddbd5b5e52e0b58053659da690951d)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **enums:** report AlterColumn next to ConvertEnumColumn for the same column (CSE0014) ([0dc2562](https://github.com/senrecep/CSharpEssentials/commit/0dc256249e2004a412f8e7a82eb7135b4219b531))
* **enums:** warn with CSE0015 when a [StringEnum] enum gets no generated metadata ([b13a2e1](https://github.com/senrecep/CSharpEssentials/commit/b13a2e169fb0c88cb2550edea273d19b410f4d44)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **http:** format enums in query and route through EnumValueFormatter ([462ef1d](https://github.com/senrecep/CSharpEssentials/commit/462ef1da70821f9b2a0e761a683a85dc18d6ea47))
* **json:** add EnumConverterFactory and AddEnumConventions ([4b8a7f6](https://github.com/senrecep/CSharpEssentials/commit/4b8a7f66a05884a72fb978eb08f75c2f379cdd47)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **json:** make reflection enum metadata an explicit opt-in and fail loud for [StringEnum] without metadata ([a32e6f3](https://github.com/senrecep/CSharpEssentials/commit/a32e6f3bf7fc3fcdb6ecd1278303bc18f04d5c87)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **openapi:** warn once per enum when a document mixes string and number formats ([74ca8f3](https://github.com/senrecep/CSharpEssentials/commit/74ca8f3367cfa73c5668a82c483bade79dffb06e))
* **swashbuckle:** describe enums by the enum conventions ([7e1258e](https://github.com/senrecep/CSharpEssentials/commit/7e1258e352fbadf705ab458ffe22a892f09f66c1)), closes [#63](https://github.com/senrecep/CSharpEssentials/issues/63)
* **validation:** add enum rules that reuse the binding error text ([cb0233e](https://github.com/senrecep/CSharpEssentials/commit/cb0233e3102d1f23bae17090ba557cbf364c4a3a))


### Fixed

* **aspnetcore:** bind whitespace-only enum values to null for nullable targets ([13806b3](https://github.com/senrecep/CSharpEssentials/commit/13806b30cef79d48592a3c44ea8100dc6bc9354b))
* **aspnetcore:** keep the host output formatters when applying an enum wire format override ([eded825](https://github.com/senrecep/CSharpEssentials/commit/eded82558f07bed26464128d04f5a7be12697b94))
* **aspnetcore:** trim surrounding whitespace of enum binding values ([733f389](https://github.com/senrecep/CSharpEssentials/commit/733f38967d26a6f12ba12ae74fc0310439c83c53))
* **aspnetcore:** validate only the first value source of an unannotated MVC enum parameter ([212d2ea](https://github.com/senrecep/CSharpEssentials/commit/212d2eabe4ce8cf94c7f22c85eeb5f6b099f0685))
* **efcore:** compare text[] flags columns as enum values on PostgreSQL ([41b521b](https://github.com/senrecep/CSharpEssentials/commit/41b521bf05ae3ef84633900d65a72952436bfdcb)), closes [#64](https://github.com/senrecep/CSharpEssentials/issues/64)
* **efcore:** keep SQLite check constraints enforced when an enum conversion fails ([b6b0d4b](https://github.com/senrecep/CSharpEssentials/commit/b6b0d4bc96ab30f8b2ab6140d9957d0616c43ded))
* **efcore:** match a missing migration schema against any schema in CSE0014 ([53bf32a](https://github.com/senrecep/CSharpEssentials/commit/53bf32a324012c6488b7759ea010d9a40743696e))
* **efcore:** name the PostgreSQL flags conversion column from a short hash of the column name ([cb76c18](https://github.com/senrecep/CSharpEssentials/commit/cb76c183a9e7d2476ca567e52f7c821e03038e29))
* **efcore:** resolve ConfigureEnumConventions(null) ambiguity with OverloadResolutionPriority ([0dc031e](https://github.com/senrecep/CSharpEssentials/commit/0dc031ebaa971a37919d91deebbc95093ce1593e))
* **efcore:** split SQLite flags text with a recursive CTE instead of json_each ([b4d9f51](https://github.com/senrecep/CSharpEssentials/commit/b4d9f510fda32d08ecbc667b65acebb7100ab827))
* **efcore:** throw when an obsolete CanConvert predicate selects an enum without metadata ([5795fd0](https://github.com/senrecep/CSharpEssentials/commit/5795fd06b2f8053ebe7574911e35d5582ad8b8eb))
* **efcore:** write enum wire names in JSON columns on EF Core 9 ([44ccbac](https://github.com/senrecep/CSharpEssentials/commit/44ccbac3031855d489c23773c95117df75a23cca)), closes [#64](https://github.com/senrecep/CSharpEssentials/issues/64)
* **enums:** escape keyword member names in generated value expressions ([a468a28](https://github.com/senrecep/CSharpEssentials/commit/a468a28f252b76ff34b61c850c4800ebaa975104)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **enums:** format a flags zero without a zero member as "0" ([fac8d4a](https://github.com/senrecep/CSharpEssentials/commit/fac8d4a8eeeb335c077c34df464766d6144bd884)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **enums:** forward IsExternalInit on .NET 5+ targets ([b4591c3](https://github.com/senrecep/CSharpEssentials/commit/b4591c3898c15c2e7165095e01d4db354274d68b)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **enums:** register metadata lazily and rerun module initializers on every miss ([af0fa89](https://github.com/senrecep/CSharpEssentials/commit/af0fa896264cba285f4330886878fe002c18a9eb)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **enums:** reject numeric text longer than 20 characters in the parser as the JSON converter does ([16eefbd](https://github.com/senrecep/CSharpEssentials/commit/16eefbd78f152bf84311bc086af0f6e8210755d1))
* **enums:** throw from EnumValueFormatter when a handled enum has no generated metadata ([68c8aa8](https://github.com/senrecep/CSharpEssentials/commit/68c8aa87107f7d78417626e86d0831fb07e6ec25))
* **enums:** use unique hint names and report colliding extensions classes with CSE0016 ([d8a5aa4](https://github.com/senrecep/CSharpEssentials/commit/d8a5aa4748318f99dedcd3add598259c4b203473)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **examples:** read the sample database password from the environment instead of appsettings ([8d504b2](https://github.com/senrecep/CSharpEssentials/commit/8d504b2fa4eb79ccb3228db730d5c3c5d8c2cf29))
* **http:** annotate reflection based ToQueryString(object) with trimming attributes ([e399d2e](https://github.com/senrecep/CSharpEssentials/commit/e399d2e07cd86d08f5752c18b8d4761a8cbf1c9b))
* **http:** annotate the remaining reflection based query string overloads for trimming ([de2395c](https://github.com/senrecep/CSharpEssentials/commit/de2395cb637534e4695851636a0e5c0873d67365))
* **http:** cache builder JSON options per EnumConventions instance with a weak table ([5a2c88b](https://github.com/senrecep/CSharpEssentials/commit/5a2c88b797cc0c947c246e483b06c592f8b3be34))
* **http:** format non-enum collection items per item with invariant culture in query strings ([a7717bb](https://github.com/senrecep/CSharpEssentials/commit/a7717bb8fb19b27afef73f01f1a27460199a8769))
* **http:** serialize WithJsonContent payload at Build so later WithEnumConventions applies ([e1f15d0](https://github.com/senrecep/CSharpEssentials/commit/e1f15d05d6d15300533ad0dfb35c20236db8b4b2))
* **json:** do not claim a StringEnum enum without metadata that CanHandle excludes ([4e67b63](https://github.com/senrecep/CSharpEssentials/commit/4e67b6379ed142f11c4b35154839689a1c66b5d8))
* **json:** keep existing JsonStringEnumConverter when adding enum conventions ([47fe470](https://github.com/senrecep/CSharpEssentials/commit/47fe4700e93169c0625c22ecc3faf0d03d240f56))
* **json:** reject over-long enum strings before unescaping and truncate error values ([282f244](https://github.com/senrecep/CSharpEssentials/commit/282f244e1bc5e4a28edf3a0704f402bf532d6401)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **json:** reject the JSON number -0 for enums and bound number error values ([025c426](https://github.com/senrecep/CSharpEssentials/commit/025c4269317a96a6683aae7faafe74c510b02fad)), closes [#61](https://github.com/senrecep/CSharpEssentials/issues/61)
* **json:** stop writing parse failures to the console from ConvertToJsonDocument ([4c9c9ba](https://github.com/senrecep/CSharpEssentials/commit/4c9c9ba51e885ad937868761fe611bfbc3f52f7e))
* **openapi:** classify MVC enum parameters by their model type ([9f8c603](https://github.com/senrecep/CSharpEssentials/commit/9f8c60385d037b63f05d95b03b6322306b411c32))
* **openapi:** describe enum properties declared on polymorphic derived types ([0b5effa](https://github.com/senrecep/CSharpEssentials/commit/0b5effab939b7ace29acdda1abdfa70480310335))
* **openapi:** fall back to WriteAs when disposing the selector scope throws ([fdd6694](https://github.com/senrecep/CSharpEssentials/commit/fdd6694ddfcddce700d10570aeb89f5f0009c071))
* **openapi:** run header format selectors in a scope and fall back to WriteAs when they throw ([cdad36b](https://github.com/senrecep/CSharpEssentials/commit/cdad36bdcfd80323e1897de0a058a42e870f60b7))
* **openapi:** write ulong enum values above long.MaxValue exactly in both packages ([45267a4](https://github.com/senrecep/CSharpEssentials/commit/45267a48b84bb6af32bde447980cbb15e26a9c48))
* **validation:** use the {Prop}.{Rule} format for enum validator error codes ([89521bf](https://github.com/senrecep/CSharpEssentials/commit/89521bf873a9c815d114f1d5602c00f3c867de09))


### Changed

* **aspnetcore:** copy request query and form only when an enum binding value changes ([611e8ae](https://github.com/senrecep/CSharpEssentials/commit/611e8ae4bd93ea5dec4a90484c5cc4fa8a1cad81))
* **efcore:** drop the per-write closure in EnumColumnCodec ([4ff6b20](https://github.com/senrecep/CSharpEssentials/commit/4ff6b20f0fda51a671f9df59a054ada103b86de6))
* **json:** drop the per-call delegate array in ConvertToJsonDocument ([fe2c5a6](https://github.com/senrecep/CSharpEssentials/commit/fe2c5a62e3c30c572afd7c23383aa1d8677f5760))
* **json:** parse a number token once in the enum converter ([4823407](https://github.com/senrecep/CSharpEssentials/commit/48234078cc07f06e3db818c85e65a46724a7a541))
* **json:** read an over-long unknown enum value as the fallback without allocating its string ([70ea029](https://github.com/senrecep/CSharpEssentials/commit/70ea02931e4948e2dbe4ea835869e39bfc404883))


### Build

* **enums:** let every csharpessentials package bring the enums generator and analyzers ([0fff7c3](https://github.com/senrecep/CSharpEssentials/commit/0fff7c305a1b94047ebf1df26374e9388f750e19))
* **enums:** let the enums generator and analyzers flow through errors, results, openapi and swashbuckle ([0d4a4e3](https://github.com/senrecep/CSharpEssentials/commit/0d4a4e376cd4c7a428faf5df1abd957c0b3f0a46))
* **enums:** let the enums generator and analyzers flow through json, efcore, aspnetcore, http and the meta-package ([49890fc](https://github.com/senrecep/CSharpEssentials/commit/49890fc6a098014e3409eb731e7032b1b452e78e))

## [4.1.0](https://github.com/senrecep/CSharpEssentials/compare/v4.0.0...v4.1.0) (2026-10-06)

### Added

- New package `CSharpEssentials.Endpoints`: source-generated endpoint registries (`IEndpoint`, `IEndpointGroup`, `[EndpointGroup]`), a reflection fallback (`MapEndpointsFromAssemblies`), `RouteOf<TEndpoint>`, `RequireRoles`/`RequirePolicies`/`RequireAuthSchemes`, and analyzers CSE1001 to CSE1011 with a code fix for CSE1004. See the [Readme](CSharpEssentials.Endpoints/Readme.MD), including a migration guide from Carter.
- New package `CSharpEssentials.DependencyInjection`: `[RegisterScoped]`/`[RegisterSingleton]`/`[RegisterTransient]` with keys, `As` and `Strategy`, `[Decorates]`, runtime `Decorate`/`TryDecorate`, a reflection fallback (`AddServicesFromAssemblies`), and analyzers CSE2001 to CSE2009 with a code fix for CSE2003. See the [Readme](CSharpEssentials.DependencyInjection/Readme.MD), including a migration guide from Scrutor.
- `CSharpEssentials.AspNetCore`: `MapVersionedGroup(version)` and the `WithValidation<T>()` endpoint filter for `CSharpEssentials.Validation` validators. The package now depends on `CSharpEssentials.Validation`.
- `CSharpEssentials.Core`: `IsBetween` (inclusive) and `IsBetweenExclusive` for any `IComparable<T>`.
- `CSharpEssentials.Time`: `NextDayOfWeek`/`PreviousDayOfWeek` for `DateTime` and `DateOnly`, and `GetAge(DateOnly, DateOnly)`/`GetAge(DateOnly, IDateTimeProvider)`.
- `CSharpEssentials.Json`: `JsonElement.ToClrObject()` converts a `JsonElement` to dictionaries, lists and CLR primitives.
- A Native AOT example for Endpoints and DependencyInjection, published in CI.

### Known issues

- The `net11.0` assets are built with the .NET 11 RC1 SDK. A patch release rebuilt with the .NET 11 GA SDK follows the GA release.

## [4.0.0](https://github.com/senrecep/CSharpEssentials/compare/v3.2.3...v4.0.0) (2026-10-06)

4.0 changes default behavior in `CSharpEssentials.AspNetCore`, `CSharpEssentials.EntityFrameworkCore` and `CSharpEssentials.Resilience`. Each old behavior can be turned back on with a setting. Read [Migrating from 3.x to 4.0](docs/migration/v3-to-v4.md) before upgrading.

### Added

- Enum query and route binding for Minimal API and MVC: `services.AddEnumBinding()` and `app.UseEnumBinding()`.
- `IExceptionProblemMapper`, `IErrorStatusCodeMapper` and `IProblemDetailsEnricher` extension points for ProblemDetails.
- `services.ConfigureInvalidModelStateResponse()` writes the automatic `[ApiController]` 400 as an enhanced problem.
- `ConditionalStringEnumConverter.AllowUndefinedValues`.
- `RetryIfFailed` overloads with a `Func<Error, bool> shouldRetry` predicate.
- `SecretManagerConfigurationOptions.LoggerFactory` for GCP Secret Manager logging.
- Analyzer CSE0001 for `[StringEnum]` enums nested in a class or struct.

### Changed

- ProblemDetails output: W3C trace id, fewer request fields by default, RFC 9110 `type` URIs, validation-only `errors`, exception messages hidden in 500 responses. `o.UseLegacyDefaults()` restores the 3.x fields.
- Minimal API, MVC, `GlobalExceptionHandler` and status code pages all write ProblemDetails through `IProblemDetailsService`.
- `ResultEndpointFilter` returns ProblemDetails instead of a raw `Error[]`.
- `StringEnumNaming` is the single enum naming source for JSON, EF Core, Swagger and binding. `UseLegacySnakeCase` restores the 3.x EF Core spelling.
- The non-generic `ResiliencePolicy` retries failed `Result` values, and caller cancellation throws `OperationCanceledException`.
- `CSharpEssentials.Resilience` depends on `Polly.Core` instead of `Polly`. `CSharpEssentials.GcpSecretManager` depends on `CSharpEssentials.Resilience`.
- `Swashbuckle.AspNetCore` is bounded to `[8.1.0, 10)`, and `CSharpEssentials.EntityFrameworkCore` pins each target framework to its own EF Core major.
- `SecretManagerConfigurationOptions` is a `sealed record` (binary-breaking for `AddProject`).

### Fixed

- `PolymorphicJsonConverterFactory` no longer wraps collection and dictionary interfaces in a `$type` envelope.
- `JsonOptions.ApplyTo` keeps the target's `TypeInfoResolver`.
- GCP Secret Manager retries listing and secret access on transient errors, and loads secrets without a shared mutable dictionary.
- `AddValidator` and `AddValidatorsFromAssembly` no longer register the same validator twice.
- `default(ResiliencePolicy)` behaves like `Create()`.

## [3.2.3](https://github.com/senrecep/CSharpEssentials/compare/v3.2.2...v3.2.3) (2026-05-31)

### Added

- `CSharpEssentials.These` is part of the meta-package.

## [3.2.2](https://github.com/senrecep/CSharpEssentials/compare/v3.2.1...v3.2.2) (2026-05-31)

### Added

- New package `CSharpEssentials.These` with JSON serialization.
- More `Maybe` and `Result` extensions, `RuleEngine.FromPredicate` and `FakeDateTimeProvider`.

## [3.2.1](https://github.com/senrecep/CSharpEssentials/compare/v3.2.0...v3.2.1) (2026-05-31)

### Added

- `ExceptionHandlingBehavior` pipeline behavior in `CSharpEssentials.Mediator`.

## [3.2.0](https://github.com/senrecep/CSharpEssentials/compare/v3.1.0...v3.2.0) (2026-05-31)

### Added

- New package `CSharpEssentials.Resilience`.

### Fixed

- `CSharpEssentials.Http` restores the default 30 second timeout.
- The `[StringEnum]` generator skips nested enums instead of producing invalid code.

## [3.1.0](https://github.com/senrecep/CSharpEssentials/compare/v3.0.8...v3.1.0) (2026-05-27)

### Added

- Batch APIs for `Result`, `Maybe` and `Any`.
- Railway validation bindings on `Result`, `Task` and `ValueTask`.

### Changed

- Failure paths of `Result` pipelines no longer allocate.

## [3.0.8](https://github.com/senrecep/CSharpEssentials/compare/v3.0.7...v3.0.8) (2026-05-20)

### Changed

- `CSharpEssentials.Validation` allocates less on the valid path.

## [3.0.7](https://github.com/senrecep/CSharpEssentials/compare/v3.0.6...v3.0.7) (2026-05-20)

### Fixed

- Validators support nullable types and concrete collections.

## [3.0.6](https://github.com/senrecep/CSharpEssentials/compare/v3.0.5...v3.0.6) (2026-05-20)

### Added

- New package `CSharpEssentials.Validation`. `CSharpEssentials.Mediator` uses it and surfaces validation errors based on `TResponse`.

## [3.0.5](https://github.com/senrecep/CSharpEssentials/compare/v3.0.4...v3.0.5) (2026-05-07)

### Fixed

- Static analysis warnings across the packages.

## [3.0.4](https://github.com/senrecep/CSharpEssentials/compare/v3.0.3...v3.0.4) (2026-05-06)

### Added

- GitHub Pages landing page and per-package AI agent skills under `.well-known/agent-skills`.

## [3.0.3](https://github.com/senrecep/CSharpEssentials/compare/v3.0.2...v3.0.3) (2026-05-06)

### Added

- New package `CSharpEssentials.Mediator` with pipeline behaviors.
- CQRS `DbContext` registration helpers and SQL connection factory abstractions in `CSharpEssentials.EntityFrameworkCore`.
- Endpoint route builder versioning extensions in `CSharpEssentials.AspNetCore`.
- Redirect following in `CSharpEssentials.Http`.
- `Result.Try` and `Result.TryAsync` overloads.

## [3.0.2](https://github.com/senrecep/CSharpEssentials/compare/v3.0.1...v3.0.2) (2026-05-05)

### Added

- New package `CSharpEssentials.Http`.
- `[StringEnum]` incremental source generator.
- `ResultEndpointFilter` for Minimal APIs.
- `Combine`, `Recover` and `Unwrap` for `Result`.

## [3.0.1](https://github.com/senrecep/CSharpEssentials/compare/v3.0.0...v3.0.1) (2026-05-04)

### Fixed

- Package metadata.

## [3.0.0](https://github.com/senrecep/CSharpEssentials/releases/tag/v3.0.0) (2026-05-04)

### Added

- `Result`: `Ensure`, `MapError`, `TapError`, `ElseDo`, `Compensate`, `Deconstruct`, `Try`, `SuccessIf`, `FailureIf`, `ThenEnsure`, `Select`/`SelectMany`.
- `Maybe`: `Tap`, `TapIf`, `BindIf`, `MapIf`, `ToResult`, `AsMaybe`.
- `Any`: `Deconstruct`, `ToTuple`, `Is<T>`, `As<T>`, `TryAs<T>`.
- `Error`: implicit array conversion and an error combination operator.
- SourceLink and symbol packages.
